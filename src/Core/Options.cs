﻿namespace Oxide.Plugins
{
  using System.Collections.Generic;

  public partial class Imperium : RustPlugin
  {
    class ImperiumOptions
    {
      public bool EnableAreaClaims;
      public bool EnableBadlands;
      public bool EnableDecayReduction;
      public bool EnableDefensiveBonuses;
      public bool EnableRestrictedPVP;
      public bool EnableTaxation;
      public bool EnableTowns;
      public bool EnableUpkeep;
      public bool EnableWar;
      public int MinFactionMembers;
      public int MinAreaNameLength;
      public int MinCassusBelliLength;
      public float DefaultTaxRate;
      public float MaxTaxRate;
      public float ClaimedLandGatherBonus;
      public float TownGatherBonus;
      public float BadlandsGatherBonus;
      public float ClaimedLandDecayReduction;
      public float TownDecayReduction;
      public List<int> ClaimCosts = new List<int>();
      public List<int> UpkeepCosts = new List<int>();
      public List<float> DefensiveBonuses = new List<float>();
      public HashSet<string> DangerousMonuments = new HashSet<string>();
      public int ZoneDomeDarkness;
      public float EventZoneRadius;
      public float EventZoneLifespanSeconds;
      public int UpkeepCheckIntervalMinutes;
      public int UpkeepCollectionPeriodHours;
      public int UpkeepGracePeriodHours;
      public string MapImageUrl;
      public int MapImageSize;
      public int CommandCooldownSeconds;
    }

    protected override void LoadDefaultConfig()
    {
      PrintWarning("Loading default configuration.");

      var options = new ImperiumOptions {
        EnableAreaClaims = true,
        EnableBadlands = true,
        EnableDecayReduction = true,
        EnableDefensiveBonuses = true,
        EnableRestrictedPVP = false,
        EnableTaxation = true,
        EnableTowns = true,
        EnableUpkeep = true,
        EnableWar = true,
        MinFactionMembers = 3,
        MinAreaNameLength = 3,
        MinCassusBelliLength = 50,
        DefaultTaxRate = 0.1f,
        MaxTaxRate = 0.2f,
        ClaimedLandGatherBonus = 0.1f,
        TownGatherBonus = 0.1f,
        BadlandsGatherBonus = 0.1f,
        ClaimedLandDecayReduction = 0.5f,
        TownDecayReduction = 1f,
        ClaimCosts = new List<int> { 0, 100, 200, 300, 400, 500 },
        UpkeepCosts = new List<int> { 10, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 },
        UpkeepCheckIntervalMinutes = 15,
        UpkeepCollectionPeriodHours = 24,
        UpkeepGracePeriodHours = 12,
        DefensiveBonuses = new List<float> { 0, 0.5f, 1f },
        DangerousMonuments = new HashSet<string> {
          "airfield",
          "sphere_tank",
          "junkyard",
          "launch_site",
          "military_tunnel",
          "powerplant",
          "satellite_dish",
          "trainyard",
          "water_treatment_plant"
        },
        ZoneDomeDarkness = 3,
        EventZoneRadius = 100f,
        EventZoneLifespanSeconds = 600f,
        MapImageUrl = "",
        MapImageSize = 1440,
        CommandCooldownSeconds = 10
      };

      Config.WriteObject(options, true);
    }
  }
}
