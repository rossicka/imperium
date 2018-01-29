﻿namespace Oxide.Plugins
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using UnityEngine;

  public partial class Imperium
  {
    class FactionManager
    {
      Dictionary<string, Faction> Factions = new Dictionary<string, Faction>();
      FactionEntityMonitor EntityMonitor;

      public FactionManager()
      {
        Factions = new Dictionary<string, Faction>();
        EntityMonitor = Instance.GameObject.AddComponent<FactionEntityMonitor>();
      }

      public Faction Create(string id, User owner)
      {
        Faction faction;

        if (Factions.TryGetValue(id, out faction))
          throw new InvalidOperationException($"Cannot create a new faction named ${id}, since one already exists");

        faction = new Faction(id, owner);
        Factions.Add(id, faction);

        Api.HandleFactionCreated(faction);

        return faction;
      }

      // TODO: Move some of this into a hook?
      public void Disband(Faction faction)
      {
        Area[] areas = Instance.Areas.GetAllClaimedByFaction(faction);

        if (areas.Length > 0)
        {
          foreach (Area area in areas)
            Instance.PrintToChat(Messages.AreaClaimLostFactionDisbandedAnnouncement, area.FactionId, area.Id);

          Instance.Areas.Unclaim(areas);
        }

        Instance.Wars.EndAllWarsForEliminatedFactions();

        foreach (User user in faction.GetAllActiveMembers())
          user.SetFaction(null);

        Factions.Remove(faction.Id);
        Api.HandleFactionDisbanded(faction);

        Instance.OnFactionsChanged();
      }

      public Faction[] GetAll()
      {
        return Factions.Values.ToArray();
      }

      public Faction Get(string id)
      {
        Faction faction;
        if (Factions.TryGetValue(id, out faction))
          return faction;
        else
          return null;
      }

      public bool Exists(string id)
      {
        return Factions.ContainsKey(id);
      }

      public Faction GetByMember(User user)
      {
        return GetByMember(user.Id);
      }

      public Faction GetByMember(string userId)
      {
        return Factions.Values.Where(f => f.HasMember(userId)).FirstOrDefault();
      }

      public Faction GetByTaxChest(StorageContainer container)
      {
        return GetByTaxChest(container.net.ID);
      }

      public Faction GetByTaxChest(uint containerId)
      {
        return Factions.Values.SingleOrDefault(f => f.TaxChest != null && f.TaxChest.net.ID == containerId);
      }

      public void SetTaxRate(Faction faction, float taxRate)
      {
        faction.TaxRate = taxRate;
        Instance.OnFactionsChanged();
      }

      public void SetTaxChest(Faction faction, StorageContainer taxChest)
      {
        faction.TaxChest = taxChest;
        Instance.OnFactionsChanged();
      }

      public void Init(IEnumerable<FactionInfo> factionInfos)
      {
        Instance.Puts($"Creating factions for {factionInfos.Count()} factions...");

        foreach (FactionInfo info in factionInfos)
        {
          Faction faction = new Faction(info);
          Factions.Add(faction.Id, faction);
        }

        Instance.Puts("Factions created.");
      }

      public void Destroy()
      {
        UnityEngine.Object.Destroy(EntityMonitor);
        Factions.Clear();
      }

      public FactionInfo[] Serialize()
      {
        return Factions.Values.Select(faction => faction.Serialize()).ToArray();
      }
    }
  }
}