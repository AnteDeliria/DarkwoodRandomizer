using System.Collections.Generic;

namespace DarkwoodRandomizer.Plugin.Pools
{
    internal static class DreamPools
    {
        internal static readonly List<string> ALL_DREAMS =
            ["dream_acid", "dream_bunker_underground_01", "dream_church_ruins_01", "dream_doctor_01",
            "dream_doctor_02", "dream_grave_meadow", "dream_grave_screenshot", "dream_home",
            "dream_oneChance_01_2", "dream_tutorial_00", "dream_tutorial_01", "dream_undergroundCh2_01",
            "dream_village_cellar", "epilog_part1a_dream", "epilog_part1b_corridor_dream", "epilog_part1c_room_dream",
            "epilog_part1c_room_dream_returnFromCrater", "epilog_part2_crater", "outside_roadToHome_01", "prolog_tut_01_doc_screenshot"];

        internal static readonly List<string> PROLOGUE_DREAMS =
            ["dream_tutorial_00", "dream_tutorial_01"];

        internal static readonly List<string> LEVEL_UP_DREAMS =
            ["dream_acid", "dream_bunker_underground_01", "dream_church_ruins_01",
            "dream_grave_meadow", "dream_home"];

        internal static readonly List<string> EPILOGUE_DREAMS =
            ["epilog_part1a_dream", "epilog_part1b_corridor_dream", "epilog_part1c_room_dream",
            "epilog_part1c_room_dream_returnFromCrater", "epilog_part2_crater", "outside_roadToHome_01"];

        internal static readonly List<string> STORY_DREAMS =
            ["dream_doctor_01", "dream_doctor_02", "dream_village_cellar"];

        internal static readonly List<string> UNUSED_DREAMS =
            ["dream_oneChance_01_2"];

        internal static readonly List<string> NON_BROKEN_DREAMS =
            ["dream_acid", "dream_bunker_underground_01", "dream_church_ruins_01", "dream_doctor_01",
            "dream_doctor_02", "dream_grave_meadow", "dream_home", "dream_oneChance_01_2",
            "dream_tutorial_00", "dream_tutorial_01", "dream_village_cellar", "epilog_part1a_dream",
            "epilog_part1b_corridor_dream", "epilog_part1c_room_dream", "epilog_part1c_room_dream_returnFromCrater", "epilog_part2_crater",
            "outside_roadToHome_01"];
    }
}
