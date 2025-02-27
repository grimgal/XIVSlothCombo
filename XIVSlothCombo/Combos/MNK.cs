using System;
using System.Linq;
using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVSlothComboPlugin.Combos
{
    internal static class MNK
    {
        public const byte ClassID = 2;
        public const byte JobID = 20;

        public const uint
            Bootshine = 53,
            DragonKick = 74,
            SnapPunch = 56,
            TwinSnakes = 61,
            Demolish = 66,
            ArmOfTheDestroyer = 62,
            Rockbreaker = 70,
            FourPointFury = 16473,
            PerfectBalance = 69,
            TrueStrike = 54,
            Meditation = 3546,
            HowlingFist = 25763,
            Enlightenment = 16474,
            MasterfulBlitz = 25764,
            ElixirField = 3545,
            FlintStrike = 25882,
            RisingPhoenix = 25768,
            ShadowOfTheDestroyer = 25767,
            RiddleOfFire = 7395,
            RiddleOfWind = 25766,
            Brotherhood = 7396,
            ForbiddenChakra = 3546,
            FormShift = 4262,
            Thunderclap = 25762;


        public static class Buffs
        {
            public const ushort
                TwinSnakes = 101,
                OpoOpoForm = 107,
                RaptorForm = 108,
                CoerlForm = 109,
                PerfectBalance = 110,
                RiddleOfFire = 1181,
                LeadenFist = 1861,
                FormlessFist = 2513,
                DisciplinedFist = 3001,
                Brotherhood = 1185;
        }

        public static class Debuffs
        {
            public const ushort
                Demolish = 246;
        }

        public static class Levels
        {
            public const byte
                TrueStrike = 4,
                SnapPunch = 6,
                Meditation = 15,
                TwinSnakes = 18,
                ArmOfTheDestroyer = 26,
                Rockbreaker = 30,
                Demolish = 30,
                FourPointFury = 45,
                HowlingFist = 40,
                DragonKick = 50,
                PerfectBalance = 50,
                FormShift = 52,
                MasterfulBlitz = 60,
                RiddleOfFire = 68,
                Enlightenment = 70,
                Brotherhood = 70,
                RiddleOfWind = 72,
                ShadowOfTheDestroyer = 82;
        }
        public static class Config
        {
            public const string
                MnkDemolishApply = "MnkDemolishApply";
            public const string
                MnkDisciplinedFistApply = "MnkDisciplinedFistApply";
        }


        internal class MnkAoECombo : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MnkArmOfTheDestroyerCombo;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID == ArmOfTheDestroyer || actionID == ShadowOfTheDestroyer)
                {
                    var inCombat = HasCondition(Dalamud.Game.ClientState.Conditions.ConditionFlag.InCombat);
                    var gauge = GetJobGauge<MNKGauge>();
                    var canWeave = CanWeave(actionID, 0.5);
                    var canWeaveChakra = CanWeave(actionID);

                    var pbStacks = FindEffectAny(Buffs.PerfectBalance);
                    var lunarNadi = gauge.Nadi == Nadi.LUNAR;
                    var solarNadi = gauge.Nadi == Nadi.SOLAR;
                    var nadiNONE = gauge.Nadi == Nadi.NONE;

                    if (!inCombat)
                    {
                        if (gauge.Chakra < 5)
                        {
                            return Meditation;
                        }
                        if (level >= Levels.FormShift && !HasEffect(Buffs.FormlessFist) && comboTime <= 0)
                        {
                            return FormShift;
                        }
                        if (IsEnabled(CustomComboPreset.MnkThunderclapOnAoEComboFeature) && !InMeleeRange() && gauge.Chakra == 5 && HasEffect(Buffs.FormlessFist))
                        {
                            return Thunderclap;
                        }
                    }

                    // Buffs
                    if (inCombat && canWeave)
                    {
                        if (IsEnabled(CustomComboPreset.MnkCDsOnAoEComboFeature))
                        {
                            if (level >= Levels.RiddleOfFire && !IsOnCooldown(RiddleOfFire))
                            {
                                return RiddleOfFire;
                            }
                            if (IsEnabled(CustomComboPreset.MnkPerfectBalanceOnAoEComboFeature) &&
                                level >= Levels.PerfectBalance && !HasEffect(Buffs.PerfectBalance) && OriginalHook(MasterfulBlitz) == MasterfulBlitz)
                            {
                                // Use Perfect Balance if:
                                // 1. It's after Bootshine/Dragon Kick.
                                // 2. At max stacks / before overcap.
                                // 3. During Brotherhood.
                                // 4. During Riddle of Fire.
                                // 5. Prepare Masterful Blitz for the Riddle of Fire & Brotherhood window.
                                if (((GetRemainingCharges(PerfectBalance) == 2) ||
                                    (GetRemainingCharges(PerfectBalance) == 1 && GetCooldownChargeRemainingTime(PerfectBalance) < 4) ||
                                    (GetRemainingCharges(PerfectBalance) >= 1 && HasEffect(Buffs.Brotherhood)) ||
                                    (GetRemainingCharges(PerfectBalance) >= 1 && HasEffect(Buffs.RiddleOfFire) && FindEffect(Buffs.RiddleOfFire).RemainingTime < 10) ||
                                    (GetRemainingCharges(PerfectBalance) >= 1 && GetCooldownRemainingTime(RiddleOfFire) < 4 && GetCooldownRemainingTime(Brotherhood) < 8)))
                                {
                                    return PerfectBalance;
                                }
                            }
                            if (IsEnabled(CustomComboPreset.MnkBrotherhoodOnAoEComboFeature) && level >= Levels.Brotherhood && !IsOnCooldown(Brotherhood))
                            {
                                return Brotherhood;
                            }
                            if (IsEnabled(CustomComboPreset.MnkRiddleOfWindOnAoEComboFeature) && level >= Levels.RiddleOfWind && !IsOnCooldown(RiddleOfWind))
                            {
                                return RiddleOfWind;
                            }
                        }
                        if (IsEnabled(CustomComboPreset.MnkMeditationOnAoEComboFeature) && level >= Levels.Meditation && gauge.Chakra == 5 && HasEffect(Buffs.DisciplinedFist) && canWeaveChakra)
                        {
                            return level >= Levels.Enlightenment ? OriginalHook(Enlightenment) : OriginalHook(Meditation);
                        }
                    }

                    // Masterful Blitz
                    if (IsEnabled(CustomComboPreset.MonkMasterfulBlitzOnAoECombo) &&
                        level >= Levels.MasterfulBlitz && !HasEffect(Buffs.PerfectBalance) && OriginalHook(MasterfulBlitz) != MasterfulBlitz)
                    {
                        return OriginalHook(MasterfulBlitz);
                    }

                    // Perfect Balance
                    if (HasEffect(Buffs.PerfectBalance))
                    {
                        if (nadiNONE || !lunarNadi)
                        {
                            if (pbStacks.StackCount > 0)
                            {
                                return level >= Levels.ShadowOfTheDestroyer ? ShadowOfTheDestroyer : Rockbreaker;
                            }
                        }
                        if (lunarNadi)
                        {
                            switch (pbStacks.StackCount)
                            {
                                case 3:
                                    return OriginalHook(ArmOfTheDestroyer);
                                case 2:
                                    return FourPointFury;
                                case 1:
                                    return Rockbreaker;
                            }
                        }
                    }

                    // Monk Rotation
                    if (HasEffect(Buffs.OpoOpoForm))
                    {
                        return OriginalHook(ArmOfTheDestroyer);
                    }

                    if (HasEffect(Buffs.RaptorForm) && level >= Levels.FourPointFury)
                    {
                        return FourPointFury;
                    }

                    if (HasEffect(Buffs.CoerlForm) && level >= Levels.Rockbreaker)
                    {
                        return Rockbreaker;
                    }
                }
                return actionID;
            }
        }

        internal class MnkBootshineFeature : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MnkBootshineFeature;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID == DragonKick)
                {
                    if (IsEnabled(CustomComboPreset.MnkBootshineBalanceFeature) && OriginalHook(MasterfulBlitz) != MasterfulBlitz)
                        return OriginalHook(MasterfulBlitz);

                    if (HasEffect(Buffs.LeadenFist) && (
                        HasEffect(Buffs.FormlessFist) || HasEffect(Buffs.PerfectBalance) ||
                        HasEffect(Buffs.OpoOpoForm) || HasEffect(Buffs.RaptorForm) || HasEffect(Buffs.CoerlForm)))
                        return Bootshine;

                    if (level < Levels.DragonKick)
                        return Bootshine;
                }

                return actionID;
            }
        }

        internal class MnkTwinSnakesFeature : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MnkTwinSnakesFeature;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID == TrueStrike)
                {
                    var disciplinedFistBuff = HasEffect(Buffs.DisciplinedFist);
                    var disciplinedFistDuration = FindEffect(Buffs.DisciplinedFist);

                    if (level >= Levels.TrueStrike)
                    {
                        if ((!disciplinedFistBuff && level >= Levels.TwinSnakes) || (disciplinedFistDuration.RemainingTime < 6 && level >= Levels.TwinSnakes))
                            return TwinSnakes;
                        return TrueStrike;
                    }

                }

                return actionID;
            }
        }

        internal class MnkBasicCombo : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MnkBasicCombo;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID == Bootshine)
                {
                    var gauge = GetJobGauge<MNKGauge>();
                    if (HasEffect(Buffs.RaptorForm) && level >= Levels.TrueStrike)
                    {
                        if (!HasEffect(Buffs.DisciplinedFist) && level >= Levels.TwinSnakes)
                            return TwinSnakes;
                        return TrueStrike;
                    }

                    if (HasEffect(Buffs.CoerlForm) && level >= Levels.SnapPunch)
                    {
                        if (!TargetHasEffect(Debuffs.Demolish) && level >= Levels.Demolish)
                            return Demolish;
                        return SnapPunch;
                    }

                    if (!HasEffect(Buffs.LeadenFist) && HasEffect(Buffs.OpoOpoForm) && level >= Levels.DragonKick)
                        return DragonKick;
                    return Bootshine;
                }

                return actionID;
            }
        }

        internal class MonkPerfectBalanceFeature : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MonkPerfectBalanceFeature;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID == PerfectBalance)
                {
                    if (OriginalHook(MasterfulBlitz) != MasterfulBlitz && level >= 60)
                        return OriginalHook(MasterfulBlitz);
                }

                return actionID;
            }
        }
        internal class MnkBootshineCombo : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MnkBootshineCombo;

            internal static bool inOpener = false;
            internal static bool openerFinished = false;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID == Bootshine)
                {
                    var inCombat = HasCondition(Dalamud.Game.ClientState.Conditions.ConditionFlag.InCombat);
                    var gauge = GetJobGauge<MNKGauge>();
                    var canWeave = CanWeave(actionID, 0.5);
                    var canDelayedWeave = CanWeave(actionID, 0.0) && GetCooldown(actionID).CooldownRemaining < 0.7;

                    var twinsnakeDuration = FindEffect(Buffs.DisciplinedFist);
                    var demolishDuration = FindTargetEffect(Debuffs.Demolish);

                    var pbStacks = FindEffectAny(Buffs.PerfectBalance);
                    var lunarNadi = gauge.Nadi == Nadi.LUNAR;
                    var solarNadi = gauge.Nadi == Nadi.SOLAR;

                    // Opener for MNK
                    if (IsEnabled(CustomComboPreset.MnkLunarSolarOpenerOnMainComboFeature))
                    {
                        // Re-enter opener when Brotherhood is used
                        if (lastComboMove == Brotherhood)
                        {
                            inOpener = true;
                            openerFinished = false;
                        }

                        if (!inCombat)
                        {
                            if (inOpener || openerFinished)
                            {
                                inOpener = false;
                                openerFinished = false;
                            }
                        }
                        else
                        {
                            if (!inOpener && !openerFinished)
                            {
                                inOpener = true;
                            }
                        }

                        if (inCombat && inOpener && !openerFinished)
                        {
                            if (level >= Levels.RiddleOfFire)
                            {
                                // Early exit out of opener
                                if (IsOnCooldown(RiddleOfFire) && GetCooldownRemainingTime(RiddleOfFire) <= 40)
                                {
                                    inOpener = false;
                                    openerFinished = true;
                                }

                                // Delayed weave for Riddle of Fire specifically
                                if (canDelayedWeave)
                                {
                                    if ((HasEffect(Buffs.CoerlForm) || lastComboMove == TwinSnakes) && !IsOnCooldown(RiddleOfFire))
                                    {
                                        return RiddleOfFire;
                                    }
                                }

                                if (canWeave)
                                {
                                    if (IsOnCooldown(RiddleOfFire) && GetCooldownRemainingTime(RiddleOfFire) <= 59)
                                    {
                                        if (level >= Levels.Brotherhood && !IsOnCooldown(Brotherhood) && IsOnCooldown(RiddleOfFire) &&
                                           (lastComboMove == Bootshine || lastComboMove == DragonKick))
                                        {
                                            return Brotherhood;
                                        }
                                        if (GetRemainingCharges(PerfectBalance) > 0 && !HasEffect(Buffs.PerfectBalance) && !HasEffect(Buffs.FormlessFist) &&
                                           (lastComboMove == Bootshine || lastComboMove == DragonKick) && OriginalHook(MasterfulBlitz) == MasterfulBlitz)
                                        {
                                            return PerfectBalance;
                                        }
                                        if (level >= Levels.RiddleOfWind && HasEffect(Buffs.PerfectBalance) && !IsOnCooldown(RiddleOfWind))
                                        {
                                            return RiddleOfWind;
                                        }
                                        if (level >= Levels.Meditation && gauge.Chakra == 5)
                                        {
                                            return OriginalHook(Meditation);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Automatically exit opener if we don't have Riddle of Fire
                                inOpener = false;
                                openerFinished = true;
                            }
                        }
                    }

                    // Out of combat preparation
                    if (!inCombat)
                    {
                        if (!inOpener && gauge.Chakra < 5)
                        {
                            return Meditation;
                        }
                        if (!inOpener && level >= Levels.FormShift && !HasEffect(Buffs.FormlessFist) && comboTime <= 0)
                        {
                            return FormShift;
                        }
                        if (IsEnabled(CustomComboPreset.MnkThunderclapOnMainComboFeature) && !InMeleeRange() && gauge.Chakra == 5 && HasEffect(Buffs.FormlessFist))
                        {
                            return Thunderclap;
                        }
                    }

                    // Buffs
                    if (inCombat && !inOpener)
                    {
                        if (IsEnabled(CustomComboPreset.MnkCDsOnMainComboFeature))
                        {
                            if (canWeave)
                            {
                                if (IsEnabled(CustomComboPreset.MnkPerfectBalanceOnMainComboFeature) && !HasEffect(Buffs.FormlessFist) &&
                                    level >= Levels.PerfectBalance && !HasEffect(Buffs.PerfectBalance) && HasEffect(Buffs.DisciplinedFist) &&
                                    OriginalHook(MasterfulBlitz) == MasterfulBlitz)
                                {
                                    // Use Perfect Balance if:
                                    // 1. It's after Bootshine/Dragon Kick.
                                    // 2. At max stacks / before overcap.
                                    // 3. During Brotherhood.
                                    // 4. During Riddle of Fire after Demolish has been applied.
                                    // 5. Prepare Masterful Blitz for the Riddle of Fire & Brotherhood window.
                                    if ((lastComboMove == Bootshine || lastComboMove == DragonKick) &&
                                        ((GetRemainingCharges(PerfectBalance) == 2) ||
                                        (GetRemainingCharges(PerfectBalance) == 1 && GetCooldownChargeRemainingTime(PerfectBalance) < 4) ||
                                        (GetRemainingCharges(PerfectBalance) >= 1 && HasEffect(Buffs.Brotherhood)) ||
                                        (GetRemainingCharges(PerfectBalance) >= 1 && GetCooldownRemainingTime(RiddleOfFire) < 3 && GetCooldownRemainingTime(Brotherhood) > 40) ||
                                        (GetRemainingCharges(PerfectBalance) >= 1 && HasEffect(Buffs.RiddleOfFire) && FindEffect(Buffs.RiddleOfFire).RemainingTime > 6) ||
                                        (GetRemainingCharges(PerfectBalance) >= 1 && GetCooldownRemainingTime(RiddleOfFire) < 3 && GetCooldownRemainingTime(Brotherhood) < 10)))
                                    {
                                        return PerfectBalance;
                                    }
                                }
                            }

                            if (canDelayedWeave)
                            {
                                if (level >= Levels.RiddleOfFire && !IsOnCooldown(RiddleOfFire) && HasEffect(Buffs.DisciplinedFist))
                                {
                                    return RiddleOfFire;
                                }
                            }

                            if (canWeave)
                            {
                                if (IsEnabled(CustomComboPreset.MnkBrotherhoodOnMainComboFeature) && level >= Levels.Brotherhood &&
                                   !IsOnCooldown(Brotherhood) && IsOnCooldown(RiddleOfFire))
                                {
                                    return Brotherhood;
                                }

                                if (IsEnabled(CustomComboPreset.MnkRiddleOfWindOnMainComboFeature) && level >= Levels.RiddleOfWind &&
                                   !IsOnCooldown(RiddleOfWind) && IsOnCooldown(RiddleOfFire) && IsOnCooldown(Brotherhood))
                                {
                                    return RiddleOfWind;
                                }
                            }
                        }

                        if (canWeave)
                        {
                            if (IsEnabled(CustomComboPreset.MnkMeditationOnMainComboFeature) && level >= Levels.Meditation && gauge.Chakra == 5 &&
                                HasEffect(Buffs.DisciplinedFist) && IsOnCooldown(RiddleOfFire) && lastComboMove != RiddleOfFire)
                            {
                                return OriginalHook(Meditation);
                            }
                        }
                    }

                    // Masterful Blitz
                    if (IsEnabled(CustomComboPreset.MonkMasterfulBlitzOnMainCombo) &&
                        level >= Levels.MasterfulBlitz && !HasEffect(Buffs.PerfectBalance) && OriginalHook(MasterfulBlitz) != MasterfulBlitz)
                    {
                        return OriginalHook(MasterfulBlitz);
                    }

                    // Perfect Balance
                    if (HasEffect(Buffs.PerfectBalance))
                    {
                        bool opoopoChakra = Array.Exists(gauge.BeastChakra, e => e == BeastChakra.OPOOPO);
                        bool coeurlChakra = Array.Exists(gauge.BeastChakra, e => e == BeastChakra.COEURL);
                        bool raptorChakra = Array.Exists(gauge.BeastChakra, e => e == BeastChakra.RAPTOR);
                        bool canSolar = gauge.BeastChakra.Where(e => e == BeastChakra.OPOOPO).Count() != 2;
                        if (opoopoChakra)
                        {
                            if (coeurlChakra)
                            {
                                return TwinSnakes;
                            }
                            if (raptorChakra)
                            {
                                return Demolish;
                            }
                            if (lunarNadi && !solarNadi)
                            {
                                bool demolishFirst = !TargetHasEffect(Debuffs.Demolish);
                                if (!demolishFirst && HasEffect(Buffs.DisciplinedFist))
                                {
                                    demolishFirst = twinsnakeDuration.RemainingTime >= demolishDuration.RemainingTime;
                                }
                                return demolishFirst ? Demolish : TwinSnakes;
                            }
                        }
                        if (canSolar && (lunarNadi || !solarNadi))
                        {
                            if (!raptorChakra && (!HasEffect(Buffs.DisciplinedFist) || twinsnakeDuration.RemainingTime <= 2.5))
                            {
                                return TwinSnakes;
                            }
                            if (!coeurlChakra && (demolishDuration.RemainingTime <= 2.5 || !TargetHasEffect(Debuffs.Demolish)))
                            {
                                return Demolish;
                            }
                        }
                        return HasEffect(Buffs.LeadenFist) ? Bootshine : DragonKick;
                    }

                    // Monk Rotation
                    if ((level >= Levels.DragonKick && HasEffect(Buffs.OpoOpoForm)) ||
                        (HasEffect(Buffs.FormlessFist)) && !HasEffect(Buffs.LeadenFist))
                    {
                        return HasEffect(Buffs.LeadenFist) ? Bootshine : DragonKick;
                    }
                    if (level >= Levels.TrueStrike && HasEffect(Buffs.RaptorForm))
                    {
                        if (level >= Levels.TwinSnakes && (!HasEffect(Buffs.DisciplinedFist) || twinsnakeDuration.RemainingTime <= Service.Configuration.GetCustomIntValue(Config.MnkDisciplinedFistApply)))
                        {
                            return TwinSnakes;
                        }
                        return TrueStrike;
                    }
                    if (level >= Levels.SnapPunch && HasEffect(Buffs.CoerlForm))
                    {
                        if (level >= Levels.Demolish && HasEffect(Buffs.DisciplinedFist) && (!TargetHasEffect(Debuffs.Demolish) || demolishDuration.RemainingTime <= Service.Configuration.GetCustomIntValue(Config.MnkDemolishApply)))
                        {
                            return Demolish;
                        }
                        return SnapPunch;
                    }
                }
                return actionID;
            }
        }
        internal class MnkPerfectBalancePlus : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MnkPerfectBalancePlus;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID == MasterfulBlitz)
                {
                    var gauge = GetJobGauge<MNKGauge>();
                    var pbStacks = FindEffectAny(Buffs.PerfectBalance);
                    var lunarNadi = gauge.Nadi == Nadi.LUNAR;
                    var nadiNONE = gauge.Nadi == Nadi.NONE;
                    if (!nadiNONE && !lunarNadi)
                    {
                        if (pbStacks.StackCount == 3)
                            return DragonKick;
                        if (pbStacks.StackCount == 2)
                            return Bootshine;
                        if (pbStacks.StackCount == 1)
                            return DragonKick;
                    }
                    if (nadiNONE)
                    {
                        if (pbStacks.StackCount == 3)
                            return DragonKick;
                        if (pbStacks.StackCount == 2)
                            return Bootshine;
                        if (pbStacks.StackCount == 1)
                            return DragonKick;
                    }
                    if (lunarNadi)
                    {
                        if (pbStacks.StackCount == 3)
                            return TwinSnakes;
                        if (pbStacks.StackCount == 2)
                            return DragonKick;
                        if (pbStacks.StackCount == 1)
                            return Demolish;
                    }

                }
                return actionID;
            }
        }
        internal class MnkRiddleOfFireBrotherhoodFeature : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MnkRiddleOfFireBrotherhoodFeature;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                var RoFCD = GetCooldown(RiddleOfFire);
                var BrotherhoodCD = GetCooldown(Brotherhood);

                if (actionID == RiddleOfFire)
                {
                    if (level >= 68 && level < 70)
                        return RiddleOfFire;
                    if (RoFCD.IsCooldown && BrotherhoodCD.IsCooldown && level >= 70)
                        return RiddleOfFire;
                    if (RoFCD.IsCooldown && !BrotherhoodCD.IsCooldown && level >= 70)
                        return Brotherhood;

                    return RiddleOfFire;
                }
                return actionID;
            }
        }
        internal class MonkHowlingFistMeditationFeature : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MonkHowlingFistMeditationFeature;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID == HowlingFist || actionID == Enlightenment)
                {
                    var gauge = GetJobGauge<MNKGauge>();

                    if (gauge.Chakra < 5)
                    {
                        return Meditation;
                    }
                }
                return actionID;
            }
        }
    }
}
