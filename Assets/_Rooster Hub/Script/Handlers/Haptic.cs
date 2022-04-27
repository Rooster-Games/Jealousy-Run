using Lofelt.NiceVibrations;

namespace RoosterHub
{
    public static class Haptic 
    {
        public static void Success()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
        }

        public static void Fail()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Failure);
        }

        public static void Warning()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Warning);
        }

        public static void Selection()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Selection);
        }

        public static void LightImpact()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
        }

        public static void SoftImpact()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);
        }

        public static void MediumImpact()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
        }

        public static void HardImpact()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
        }
    }
}