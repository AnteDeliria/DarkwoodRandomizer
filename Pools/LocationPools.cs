using System.Collections.Generic;
using System.Linq;

namespace DarkwoodRandomizer.Pools
{
    internal static class LocationPools
    {
        internal static readonly List<string> HIDEOUTS_CH1 = ["med_cottage_tree_01", "big_farm_02", "big_hideout_03"];

        internal static readonly List<string> HIDEOUTS_CH2 = ["med_hideout_04", "med_hideout_05"];

        internal static List<string> HIDEOUTS => HIDEOUTS_CH1.Concat(HIDEOUTS_CH2).ToList();

        internal static readonly List<string> NON_BORDER_LOCATIONS_CH1 =
            ["med_bunker_enter_01", "big_hunter_01", "med_musican_house_01", "big_burned_houses_01",
                "big_church_ruins_01", "big_piotrek_01", "med_musicianHideout_01", "big_pigsheds_01"];

        internal static readonly List<string> NON_BORDER_LOCATIONS_CH2 =
            ["med_mushroomGranny_01", "med_junkyard_01", "med_maskFamily_01", "big_swamp_lake_02"];

        internal static List<string> NON_BORDER_LOCATIONS => NON_BORDER_LOCATIONS_CH1.Concat(NON_BORDER_LOCATIONS_CH2).ToList();

        internal static readonly List<string> WOLF_CAMPS_CH1 = ["med_wolfman_camp_01", "med_wolfman_camp_02"];

        internal static readonly List<string> BORDER_OBJECTS_CH1 =
            ["border_main_wolfmanHideout_01", "border_main_gate_villageEntrance_01", "border_main_river_01", "border_main_cottageTrailer_01",
                "border_gate_06_meadowForest_01", "border_main_cottage_01", "border_gate_02_bridge",
                "border_gate_04_burned_cottage", "border_main_gate_doctorEntrance_01", "border_main_railroad_01", "border_main_trainWreck_01"];

        internal static readonly List<string> BORDER_OBJECTS_CH2 =
            ["border_doctor_01", "border_gate_radioTower", "border_main_corner_dziura_01", "border_main_villagersSwamp_01",
                "border_mi17_01", "border_gate_05_treeVillage", "border_burnedCottage_01", "border_gate_snail_01",
                "border_main_wolf_01"];

        internal static readonly List<string> DREAMS =
            ["dream_undergroundCh2_01", "dream_bunker_underground_01", "dream_church_ruins_01", "dream_doctor_01",
                "dream_doctor_02", "dream_grave_meadow", "dream_oneChance_01_2", "dream_village_cellar"];

        internal static readonly List<string> TUTORIAL = ["dream_tutorial_00", "dream_tutorial_01"];

        internal static readonly List<string> EPILOGUE = ["epilog_part1b_corridor_dream", "epilog_part2_crater"];

        internal static readonly List<string> OUTSIDE_LOCATIONS_CH1 =
            ["outside_bunker_underground_02", "outside_church_underground_01", "outside_church_underground_02",
            "outside_doctor_house_01", "outside_village_ch1_01", "outside_village_ch1_cottage01_underground_01", "outside_well_underground_01"];

        internal static readonly List<string> OUTSIDE_LOCATIONS_CH2 =
            ["outside_cottage_snail_01", "outside_oneChance_01", "outside_roadToHome_01", "outside_village_ch2_cellar_01"];

        internal static readonly string CHAPTER_TRANSITION = "outside_bunker_underground_part2_01";
    }
}
