using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVSlothComboPlugin.Combos
{
    internal static class DRG
    {
        public const byte ClassID = 4;
        public const byte JobID = 22;

        public const uint
            TrueNorth = 7546,
            PiercingTalon = 90,
            LanceCharge = 85,
            DragonSight = 7398,
            BattleLitany = 3557,
            Jump = 92,
            LifeSurge = 83,
            HighJump = 16478,
            MirageDive = 7399,
            BloodOfTheDragon = 3553,
            Stardiver = 16480,
            CoerthanTorment = 16477,
            DoomSpike = 86,
            SonicThrust = 7397,
            ChaosThrust = 88,
            RaidenThrust = 16479,
            TrueThrust = 75,
            Disembowel = 87,
            FangAndClaw = 3554,
            WheelingThrust = 3556,
            FullThrust = 84,
            VorpalThrust = 78,
            WyrmwindThrust = 25773,
            DraconianFury = 25770,
            ChaoticSpring = 25772,
            DragonfireDive = 96,
            SpineshatterDive = 95,
            Geirskogul = 3555,
            Nastrond = 7400,
            HeavensThrust = 25771;

        public static class Buffs
        {
            public const ushort
                TrueNorth = 1250,
                LanceCharge = 1864,
                RightEye = 1910,
                BattleLitany = 786,
                SharperFangAndClaw = 802,
                EnhancedWheelingThrust = 803,
                DiveReady = 1243,
                RaidenThrustReady = 1863,
                PowerSurge = 2720,
                LifeSurge = 116,
                DraconianFire = 1863;
        }

        public static class Debuffs
        {
            public const ushort
                ChaosThrust = 118,
                ChaoticSpring = 2719;
        }

        public static class Levels
        {
            public const byte
                VorpalThrust = 4,
                PiercingTalon = 15,
                Disembowel = 18,
                FullThrust = 26,
                LanceCharge = 30,
                Jump = 30,
                SpineshatterDive = 45,
                DragonfireDive = 50,
                ChaosThrust = 50,
                TrueNorth = 50,
                BattleLitany = 52,
                FangAndClaw = 56,
                WheelingThrust = 58,
                Geirskogul = 60,
                SonicThrust = 62,
                DragonSight = 66,
                MirageDive = 68,
                Nastrond = 70,
                CoerthanTorment = 72,
                HighJump = 74,
                RaidenThrust = 76,
                Stardiver = 80,
                DraconianFury = 82,
                ChaoticSpring = 86,
                HeavensThrust = 86;
        }


        internal class DragoonCoerthanTormentCombo : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DragoonCoerthanTormentCombo;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is DRG.CoerthanTorment)
                {
                    if (comboTime > 0)
                    {
                        if ((lastComboMove is DoomSpike or DraconianFury) && level >= Levels.SonicThrust)
                            return SonicThrust;
                        if (lastComboMove is DRG.SonicThrust && level >= Levels.CoerthanTorment)
                            return CoerthanTorment;
                    }
                    return OriginalHook(DoomSpike);
                }
                return actionID;
            }
        }

        internal class DragoonChaosThrustCombo : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DragoonChaosThrustCombo;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is ChaosThrust or ChaoticSpring)
                {

                    //Piercing Talon Uptime Feature
                    if (IsEnabled(CustomComboPreset.DragoonPiercingTalonChaosFeature) && level >= Levels.PiercingTalon)
                    {
                        if (!InMeleeRange())
                            return PiercingTalon;
                    }

                    if (comboTime > 0)
                    {
                        if ((lastComboMove is TrueThrust or RaidenThrust) && level >= Levels.Disembowel)
                            return Disembowel;

                        if (lastComboMove is DRG.Disembowel && level >= Levels.ChaosThrust)
                            return OriginalHook(ChaosThrust);
                    }

                    if (IsEnabled(CustomComboPreset.DragoonFangThrustFeature) && (HasEffect(Buffs.SharperFangAndClaw) || HasEffect(Buffs.EnhancedWheelingThrust)))
                        return WheelingThrust;

                    if (HasEffect(Buffs.SharperFangAndClaw) && level >= Levels.FangAndClaw)
                        return FangAndClaw;

                    if (HasEffect(Buffs.EnhancedWheelingThrust) && level >= Levels.WheelingThrust)
                        return WheelingThrust;

                    return TrueThrust;
                }

                return actionID;
            }
        }

        internal class DragoonFullThrustCombo : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DragoonFullThrustCombo;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is DRG.FullThrust)
                {

                    //Piercing Talon Uptime Feature
                    if (IsEnabled(CustomComboPreset.DragoonPiercingTalonFullFeature) && level >= Levels.PiercingTalon)
                    {
                        if (!InMeleeRange())
                            return PiercingTalon;
                    }

                    if (comboTime > 0)
                    {
                        if ((lastComboMove is TrueThrust or RaidenThrust) && level >= Levels.VorpalThrust)
                            return VorpalThrust;

                        if (lastComboMove is DRG.VorpalThrust && level >= Levels.FullThrust)
                            return FullThrust;
                    }

                    if (HasEffect(Buffs.SharperFangAndClaw) && level >= Levels.FangAndClaw)
                        return FangAndClaw;

                    if (HasEffect(Buffs.EnhancedWheelingThrust) && level >= Levels.WheelingThrust)
                        return WheelingThrust;

                    return OriginalHook(TrueThrust);
                }

                return actionID;
            }
        }

        internal class DragoonFullThrustComboPlus : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DragoonFullThrustComboPlus;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is DRG.FullThrust)
                {
                    var canWeave = CanWeave(actionID);

                    //Piercing Talon Uptime Feature
                    if (IsEnabled(CustomComboPreset.DragoonPiercingTalonPlusFeature) && level >= Levels.PiercingTalon)
                    {
                        if (!InMeleeRange())
                            return PiercingTalon;
                    }

                    //(High) Jump Plus Feature
                    if (canWeave)
                    {
                        if (IsEnabled(CustomComboPreset.DragoonHighJumpPlusFeature))
                        {
                            if (
                                level >= Levels.HighJump &&
                                IsOffCooldown(HighJump) && canWeave
                               ) return HighJump;

                            if (
                                level >= Levels.Jump && level <= Levels.HighJump &&
                                IsOffCooldown(Jump) && canWeave
                               ) return Jump;
                        }
                    }

                    //Life Surge Plus Feature
                    if (canWeave)
                    {
                        if (IsEnabled(CustomComboPreset.DragoonLifeSurgePlusFeature) && HasEffect(Buffs.PowerSurge) && !HasEffect(Buffs.LifeSurge) && CanWeave(actionID, 0.001) && GetRemainingCharges(LifeSurge) > 0)
                        {
                            if (lastComboMove is DRG.VorpalThrust)
                            {
                                if (HasEffect(Buffs.LanceCharge))
                                    return LifeSurge;

                                if (HasEffect(Buffs.RightEye))
                                    return LifeSurge;
                            }

                            if (HasEffect(Buffs.BattleLitany))
                            {

                                if (HasEffect(Buffs.EnhancedWheelingThrust))
                                    return LifeSurge;

                                if (HasEffect(Buffs.SharperFangAndClaw))
                                    return LifeSurge;
                            }
                        }
                    }

                    //Mirage Feature
                    if (canWeave)
                    {
                        if (IsEnabled(CustomComboPreset.DragoonMiragePlusFeature))
                        {
                            if (level >= Levels.MirageDive && HasEffect(Buffs.DiveReady) && canWeave)
                                return MirageDive;
                        }
                    }

                    var Disembowel = FindEffectAny(Buffs.PowerSurge);
                    if (comboTime > 0)
                    {
                        if ((lastComboMove is TrueThrust or RaidenThrust) && level >= Levels.Disembowel && (Disembowel is null || (Disembowel.RemainingTime < 10)))
                            return DRG.Disembowel;

                        if (lastComboMove is DRG.Disembowel && level >= Levels.ChaoticSpring)
                            return ChaoticSpring;

                        if (lastComboMove is DRG.Disembowel && level >= Levels.ChaosThrust)
                            return ChaosThrust;

                        if ((lastComboMove is TrueThrust or RaidenThrust) && level >= Levels.VorpalThrust)
                            return VorpalThrust;

                        if (lastComboMove is DRG.VorpalThrust && !HasEffect(Buffs.LifeSurge) && GetRemainingCharges(LifeSurge) > 0)
                            return LifeSurge;

                        if (lastComboMove is DRG.VorpalThrust && level >= Levels.FullThrust)
                            return FullThrust;
                    }

                    if (HasEffect(Buffs.SharperFangAndClaw) && level >= Levels.FangAndClaw)
                        return FangAndClaw;

                    if (HasEffect(Buffs.EnhancedWheelingThrust) && level >= Levels.WheelingThrust)
                        return WheelingThrust;

                    return OriginalHook(TrueThrust);
                }

                return actionID;
            }
        }

        internal class DragoonSimple : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DragoonSimple;
            internal static bool inOpener = false;
            internal static bool openerFinished = false;
            internal static byte step = 0;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                var Disembowel = FindEffect(Buffs.PowerSurge);
                var gauge = GetJobGauge<DRGGauge>();

                if (actionID is DRG.FullThrust)
                {
                    var inCombat = HasCondition(Dalamud.Game.ClientState.Conditions.ConditionFlag.InCombat);
                    var canWeave = CanWeave(actionID);

                    // Lvl88+ Opener
                    if (IsEnabled(CustomComboPreset.DragoonOpenerFeature) && level >= 88)
                    {
                        if (inCombat && HasEffect(Buffs.TrueNorth) && !inOpener)
                        {
                            inOpener = true;
                        }

                        if (!inCombat && (inOpener || openerFinished))
                        {
                            inOpener = false;
                            step = 0;
                            openerFinished = false;

                            return OriginalHook(TrueThrust);
                        }

                        if (inCombat && inOpener && !openerFinished)
                        {
                            if (step is 0)
                            {
                                if (gauge.EyeCount > 0) openerFinished = true;
                                else step++;
                            }

                            if (step is 1)
                            {
                                if (lastComboMove is DRG.TrueThrust) step++;
                                else return TrueThrust;
                            }

                            if (step is 2)
                            {
                                if (lastComboMove is DRG.Disembowel) step++;
                                else return DRG.Disembowel;
                            }

                            if (step is 3)
                            {
                                if (IsOnCooldown(LanceCharge)) step++;
                                else return LanceCharge;
                            }

                            if (step is 4)
                            {
                                if (IsOnCooldown(DragonSight)) step++;
                                else return DragonSight;
                            }

                            if (step is 5)
                            {
                                if (TargetHasEffect(Debuffs.ChaoticSpring)) step++;
                                return ChaoticSpring;
                            }

                            if (step is 6)
                            {
                                if (IsOnCooldown(BattleLitany)) step++;
                                else return BattleLitany;
                            }

                            if (step is 7)
                            {
                                if (IsOnCooldown(Geirskogul)) step++;
                                else return Geirskogul;
                            }

                            if (step is 8)
                            {
                                if (!HasEffect(Buffs.EnhancedWheelingThrust)) step++;
                                else return WheelingThrust;
                            }

                            if (step is 9)
                            {
                                if (IsOnCooldown(HighJump)) step++;
                                else return HighJump;
                            }

                            if (step is 10)
                            {
                                if (GetRemainingCharges(LifeSurge) is 0 or 1) step++;
                                else return LifeSurge;
                            }

                            if (step is 11)
                            {
                                if (!HasEffect(Buffs.SharperFangAndClaw)) step++;
                                else return FangAndClaw;
                            }

                            if (step is 12)
                            {
                                if (IsOnCooldown(DragonfireDive)) step++;
                                else return DragonfireDive;
                            }

                            if (step is 13)
                            {
                                if (lastComboMove is (RaidenThrust)) step++;
                                else return RaidenThrust;
                            }

                            if (step is 14)
                            {
                                if (GetRemainingCharges(SpineshatterDive) is 0 or 1) step++;
                                else return SpineshatterDive;
                            }

                            if (step is 15)
                            {
                                if (lastComboMove is DRG.VorpalThrust) step++;
                                else return VorpalThrust;
                            }

                            if (step is 16)
                            {
                                if (GetRemainingCharges(LifeSurge) is 0) step++;
                                else return LifeSurge;
                            }

                            if (step is 17)
                            {
                                if (IsOnCooldown(MirageDive)) step++;
                                else return MirageDive;
                            }

                            if (step is 18)
                            {
                                if (lastComboMove is DRG.HeavensThrust) step++;
                                else return HeavensThrust;
                            }

                            if (step is 19)
                            {
                                if (GetRemainingCharges(SpineshatterDive) is 0) step++;
                                else return SpineshatterDive;
                            }

                            if (step is 20)
                            {
                                if (!HasEffect(Buffs.SharperFangAndClaw)) step++;
                                else return FangAndClaw;
                            }

                            if (step is 21)
                            {
                                if (!HasEffect(Buffs.EnhancedWheelingThrust)) step++;
                                else return WheelingThrust;
                            }

                            openerFinished = true;
                        }
                    }

                    // Piercing Talon Uptime Option
                    if (IsEnabled(CustomComboPreset.DRGSimpleRangedUptimeST) && level >= Levels.PiercingTalon && !InMeleeRange())
                        return PiercingTalon;

                    //Lance Charge Feature
                    if (canWeave)
                    {
                        if (IsEnabled(CustomComboPreset.DragoonLanceFeature))
                        {
                            if (HasEffect(Buffs.PowerSurge) && canWeave)
                            {
                                if (level >= Levels.LanceCharge && IsOffCooldown(LanceCharge))
                                    return LanceCharge;
                            }
                        }
                    }

                    //Dragon Sight Feature
                    if (canWeave)
                    {
                        if (IsEnabled(CustomComboPreset.DragoonDragonSightFeature))
                        {
                            if (level >= Levels.DragonSight && HasEffect(Buffs.PowerSurge) && IsOffCooldown(DragonSight) && canWeave)
                                return DragonSight;
                        }
                    }

                    //Battle Litany Feature
                    if (CanWeave(actionID, 1.3))
                    {
                        if (IsEnabled(CustomComboPreset.DragoonLitanyFeature))
                        {
                            if (HasEffect(Buffs.PowerSurge))
                            {
                                if (level >= Levels.BattleLitany && IsOffCooldown(BattleLitany))
                                    return BattleLitany;
                            }
                        }
                    }

                    //Geirskogul and Nastrond Feature Part 1
                    if (CanWeave(actionID, 0.001))
                    {
                        if (IsEnabled(CustomComboPreset.DragoonGeirskogulNastrondFeature))
                        {
                            if (level >= Levels.Geirskogul && HasEffect(Buffs.PowerSurge) && IsOffCooldown(Geirskogul))
                                return Geirskogul;
                        }
                    }

                    //(High) Jump Feature
                    if (CanWeave(actionID, 0.5))
                    {
                        if (IsEnabled(CustomComboPreset.DragoonHighJumpFeature))
                        {
                            if (HasEffect(Buffs.PowerSurge))
                            {

                                if (level >= Levels.HighJump && IsOffCooldown(HighJump))
                                    return HighJump;

                                if (level >= Levels.Jump && level < Levels.HighJump && IsOffCooldown(Jump))
                                    return Jump;
                            }
                        }
                    }

                    //Life Surge Feature
                    if (canWeave)
                    {
                        if (IsEnabled(CustomComboPreset.DragoonLifeSurgeFeature))
                        {
                            if (HasEffect(Buffs.LanceCharge) && HasEffect(Buffs.PowerSurge) && !HasEffect(Buffs.LifeSurge) && lastComboMove is DRG.VorpalThrust && GetRemainingCharges(LifeSurge) > 0 && CanWeave(actionID, 0.001))
                                return LifeSurge;

                            if (HasEffect(Buffs.RightEye) && HasEffect(Buffs.PowerSurge) && !HasEffect(Buffs.LifeSurge) && lastComboMove is DRG.VorpalThrust && GetRemainingCharges(LifeSurge) > 0 && CanWeave(actionID, 0.001))
                                return LifeSurge;

                            if (HasEffect(Buffs.BattleLitany) && HasEffect(Buffs.PowerSurge) && !HasEffect(Buffs.LifeSurge) && lastComboMove is DRG.FangAndClaw && HasEffect(Buffs.EnhancedWheelingThrust) && GetRemainingCharges(LifeSurge) > 0 && CanWeave(actionID, 0.001))
                                return LifeSurge;

                            if (HasEffect(Buffs.BattleLitany) && HasEffect(Buffs.PowerSurge) && !HasEffect(Buffs.LifeSurge) && lastComboMove is DRG.WheelingThrust && HasEffect(Buffs.SharperFangAndClaw) && GetRemainingCharges(LifeSurge) > 0 && CanWeave(actionID, 0.001))
                                return LifeSurge;
                        }
                    }

                    //Wyrmwind Thrust Feature
                    if (canWeave)
                    {
                        if (IsEnabled(CustomComboPreset.DragoonWyrmwindFeature))
                        {
                            if (
                                gauge.FirstmindsFocusCount is 2 && canWeave
                               ) return WyrmwindThrust;
                        }
                    }

                    //Geirskogul and Nastrond Feature Part 2
                    if (canWeave)
                    {

                        if (IsEnabled(CustomComboPreset.DragoonGeirskogulNastrondFeature))
                        {
                            if (gauge.IsLOTDActive is true && level >= Levels.Nastrond && HasEffect(Buffs.PowerSurge) && IsOffCooldown(Nastrond) && CanWeave(actionID, 0.001))
                                return Nastrond;
                        }
                    }

                    //Dives under Litany and Life of the Dragon Feature
                    if (canWeave)
                    {

                        if (IsEnabled(CustomComboPreset.DragoonLifeLitanyDiveFeature))
                        {
                            if (gauge.IsLOTDActive is true && level >= Levels.DragonfireDive && HasEffect(Buffs.PowerSurge) && HasEffect(Buffs.BattleLitany) && IsOffCooldown(DragonfireDive) && canWeave)
                                return DragonfireDive;

                            if (gauge.IsLOTDActive is true && level >= Levels.Stardiver && HasEffect(Buffs.PowerSurge) && IsOffCooldown(Stardiver) && CanWeave(actionID, 1.3))
                                return Stardiver;

                            if (HasEffect(Buffs.BattleLitany) && gauge.IsLOTDActive is true && level >= Levels.SpineshatterDive && HasEffect(Buffs.PowerSurge) && GetRemainingCharges(SpineshatterDive) == 2 && canWeave)
                                return SpineshatterDive;
                        }
                    }

                    //Dives under Litany Feature
                    if (canWeave)
                    {

                        if (IsEnabled(CustomComboPreset.DragoonLitanyDiveFeature))
                        {
                            if (level >= Levels.DragonfireDive && HasEffect(Buffs.PowerSurge) && HasEffect(Buffs.BattleLitany) && IsOffCooldown(DragonfireDive) && canWeave)
                                return DragonfireDive;

                            if (gauge.IsLOTDActive is true && level >= Levels.Stardiver && HasEffect(Buffs.PowerSurge) && IsOffCooldown(Stardiver) && CanWeave(actionID, 1.5))
                                return Stardiver;

                            if (level >= Levels.SpineshatterDive && HasEffect(Buffs.PowerSurge) && HasEffect(Buffs.BattleLitany) && GetRemainingCharges(SpineshatterDive) > 0 && canWeave)
                                return SpineshatterDive;
                        }
                    }

                    //Dives Feature
                    if (canWeave)
                    {

                        if (IsEnabled(CustomComboPreset.DragoonLifeLitanyDiveFeature))
                        {
                            if (level >= Levels.DragonfireDive && HasEffect(Buffs.PowerSurge) && IsOffCooldown(DragonfireDive) && canWeave)
                                return DragonfireDive;

                            if (gauge.IsLOTDActive is true && level >= Levels.Stardiver && HasEffect(Buffs.PowerSurge) && IsOffCooldown(Stardiver) && CanWeave(actionID, 1.5))
                                return Stardiver;

                            if (level >= Levels.SpineshatterDive && HasEffect(Buffs.PowerSurge) && GetRemainingCharges(SpineshatterDive) > 0 && canWeave)
                                return SpineshatterDive;
                        }
                    }

                    //Dives under Lance Charge Feature
                    if (canWeave)
                    {

                        if (IsEnabled(CustomComboPreset.DragoonLanceDiveFeature))
                        {
                            if (level >= Levels.DragonfireDive && HasEffect(Buffs.PowerSurge) && HasEffect(Buffs.LanceCharge) && IsOffCooldown(DragonfireDive) && canWeave)
                                return DragonfireDive;

                            if (gauge.IsLOTDActive is true && level >= Levels.Stardiver && HasEffect(Buffs.PowerSurge) && IsOffCooldown(Stardiver) && CanWeave(actionID, 1.5))
                                return Stardiver;

                            if (level >= Levels.SpineshatterDive && HasEffect(Buffs.PowerSurge) && HasEffect(Buffs.LanceCharge) && GetRemainingCharges(SpineshatterDive) > 0 && canWeave)
                                return SpineshatterDive;
                        }
                    }

                    //Mirage Feature
                    if (canWeave)
                    {
                        if (IsEnabled(CustomComboPreset.DragoonMirageFeature))
                        {
                            if (level >= Levels.MirageDive && HasEffect(Buffs.PowerSurge) && HasEffect(Buffs.DiveReady) && CanWeave(actionID, 0.001))
                                return MirageDive;
                        }
                    }

                    if (comboTime > 0)
                    {
                        if ((lastComboMove is TrueThrust or RaidenThrust) && level >= Levels.Disembowel && (Disembowel is null || (Disembowel.RemainingTime < 10)))
                            return DRG.Disembowel;

                        if (lastComboMove is DRG.Disembowel && level >= Levels.ChaoticSpring)
                            return ChaoticSpring;

                        if (lastComboMove is DRG.Disembowel && level >= Levels.ChaosThrust)
                            return ChaosThrust;

                        if ((lastComboMove is TrueThrust or RaidenThrust) && level >= Levels.VorpalThrust)
                            return VorpalThrust;

                        if (lastComboMove is DRG.VorpalThrust && level >= Levels.FullThrust)
                            return FullThrust;
                    }

                    if (HasEffect(Buffs.SharperFangAndClaw) && level >= Levels.FangAndClaw)
                        return FangAndClaw;

                    if (HasEffect(Buffs.EnhancedWheelingThrust) && level >= Levels.WheelingThrust)
                        return WheelingThrust;

                    return OriginalHook(TrueThrust);
                }

                return actionID;
            }
        }

        internal class DragoonSimpleAoE : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DragoonSimpleAoE;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                var canWeave = CanWeave(actionID);
                var gauge = GetJobGauge<DRGGauge>();
                if (actionID is DRG.CoerthanTorment)
                {
                    // Piercing Talon Uptime Option
                    if (IsEnabled(CustomComboPreset.DRGSimpleRangedUptimeAoE) && level >= Levels.PiercingTalon && !InMeleeRange())
                        return PiercingTalon;

                    if (canWeave)
                    {
                        //Buffs AoE Feature
                        if (IsEnabled(CustomComboPreset.DragoonAoEBuffsFeature))
                        {

                            if (level >= Levels.LanceCharge &&
                                IsOffCooldown(LanceCharge))
                                return LanceCharge;

                            if (level >= Levels.BattleLitany &&
                                IsOffCooldown(BattleLitany))
                                return BattleLitany;

                            //Dragon Sight AoE Feature
                            if (IsEnabled(CustomComboPreset.DragoonAoEDragonSightFeature))
                            {
                                if (level >= Levels.DragonSight &&
                                    IsOffCooldown(DragonSight))
                                    return DragonSight;
                            }
                        }

                        //Geirskogul and Nastrond AoE Feature Part 1
                        if (IsEnabled(CustomComboPreset.DragoonAoEGeirskogulNastrondFeature))
                        {
                            if (level >= Levels.Geirskogul &&
                                IsOffCooldown(Geirskogul))
                                return Geirskogul;

                        }

                        //(High) Jump AoE Feature
                        if (IsEnabled(CustomComboPreset.DragoonAoEHighJumpFeature))
                        {
                            if (level >= Levels.HighJump &&
                                IsOffCooldown(HighJump) && CanWeave(actionID, 1))
                                return HighJump;

                            if (level >= Levels.Jump && level <= Levels.HighJump &&
                                IsOffCooldown(Jump) && CanWeave(actionID, 1))
                                return Jump;
                        }

                        //Life Surge AoE Feature
                        if (IsEnabled(CustomComboPreset.DragoonAoELifeSurgeFeature))
                        {
                            if ((HasEffect(Buffs.LanceCharge) || HasEffect(Buffs.RightEye)) &&
                                ((lastComboMove is DRG.CoerthanTorment && level >= Levels.CoerthanTorment) ||
                                (lastComboMove is DRG.SonicThrust && level >= Levels.SonicThrust && level <= Levels.CoerthanTorment) ||
                                (lastComboMove is DRG.DoomSpike && level <= Levels.SonicThrust)) &&
                                !HasEffect(Buffs.LifeSurge) &&
                                GetRemainingCharges(LifeSurge) > 0 &&
                                CanWeave(actionID, weaveTime: 0.3))
                                return LifeSurge;

                        }


                        //Wyrmwind Thrust AoE Feature
                        if (IsEnabled(CustomComboPreset.DragoonAoEWyrmwindFeature))
                        {
                            if (gauge.FirstmindsFocusCount is 2)
                                return WyrmwindThrust;
                        }

                        //Geirskogul and Nastrond AoE Feature Part 2
                        if (IsEnabled(CustomComboPreset.DragoonAoEGeirskogulNastrondFeature))
                        {
                            if (gauge.IsLOTDActive is true && level >= Levels.Nastrond && IsOffCooldown(Nastrond))
                                return Nastrond;

                        }

                        //Dives AoE Feature
                        if (IsEnabled(CustomComboPreset.DragoonAoEDiveFeature))
                        {
                            if (level >= Levels.DragonfireDive && IsOffCooldown(DragonfireDive))
                                return DragonfireDive;

                            if (gauge.IsLOTDActive is true && level >= Levels.Stardiver && IsOffCooldown(Stardiver) && CanWeave(actionID, 1.5))
                                return Stardiver;

                            if (level >= Levels.SpineshatterDive && GetRemainingCharges(SpineshatterDive) > 0 && canWeave)
                                return SpineshatterDive;

                        }

                        //Dives under Lance Charge
                        if (IsEnabled(CustomComboPreset.DragoonAoELanceDiveFeature))
                        {
                            if (level >= Levels.DragonfireDive && IsOffCooldown(DragonfireDive) && HasEffect(Buffs.LanceCharge))
                                return DragonfireDive;

                            if (gauge.IsLOTDActive is true && level >= Levels.Stardiver && IsOffCooldown(Stardiver) && HasEffect(Buffs.LanceCharge) && CanWeave(actionID, 1.5))
                                return Stardiver;

                            if (level >= Levels.SpineshatterDive && GetRemainingCharges(SpineshatterDive) > 0 && HasEffect(Buffs.LanceCharge))
                                return SpineshatterDive;
                        }

                        //Dives under Litany AoE Feature
                        if (IsEnabled(CustomComboPreset.DragoonAoELitanyDiveFeature))
                        {
                            if (gauge.IsLOTDActive is true && level >= Levels.DragonfireDive && HasEffect(Buffs.PowerSurge) && HasEffect(Buffs.BattleLitany) && IsOffCooldown(DragonfireDive))
                                return DragonfireDive;

                            if (gauge.IsLOTDActive is true && level >= Levels.Stardiver && HasEffect(Buffs.PowerSurge) && IsOffCooldown(Stardiver) && CanWeave(actionID, 1.3))
                                return Stardiver;

                            if (HasEffect(Buffs.BattleLitany) && gauge.IsLOTDActive is true && level >= Levels.SpineshatterDive && HasEffect(Buffs.PowerSurge) && GetRemainingCharges(SpineshatterDive) == 2)
                                return SpineshatterDive;

                        }

                        //Dives under Litany and Life of the Dragon AoE Feature
                        if (IsEnabled(CustomComboPreset.DragoonAoELifeLitanyDiveFeature))
                        {
                            if (gauge.IsLOTDActive is true && level >= Levels.DragonfireDive && HasEffect(Buffs.PowerSurge) && HasEffect(Buffs.BattleLitany) && IsOffCooldown(DragonfireDive))
                                return DragonfireDive;

                            if (gauge.IsLOTDActive is true && level >= Levels.Stardiver && HasEffect(Buffs.PowerSurge) && IsOffCooldown(Stardiver) && CanWeave(actionID, 1.3))
                                return Stardiver;

                            if (HasEffect(Buffs.BattleLitany) && gauge.IsLOTDActive is true && level >= Levels.SpineshatterDive && HasEffect(Buffs.PowerSurge) && GetRemainingCharges(SpineshatterDive) == 2)
                                return SpineshatterDive;

                        }

                        //Dives under Lance Charge AoE Feature
                        if (IsEnabled(CustomComboPreset.DragoonAoELitanyDiveFeature))
                        {
                            if (level >= Levels.DragonfireDive && HasEffect(Buffs.PowerSurge) && HasEffect(Buffs.LanceCharge) && IsOffCooldown(DragonfireDive))
                                return DragonfireDive;

                            if (gauge.IsLOTDActive is true && level >= Levels.Stardiver && HasEffect(Buffs.PowerSurge) && IsOffCooldown(Stardiver) && CanWeave(actionID, 1.5))
                                return Stardiver;

                            if (level >= Levels.SpineshatterDive && HasEffect(Buffs.PowerSurge) && HasEffect(Buffs.LanceCharge) && GetRemainingCharges(SpineshatterDive) > 0)
                                return SpineshatterDive;

                        }

                        //Mirage AoE Feature
                        if (IsEnabled(CustomComboPreset.DragoonAoEMirageFeature))
                        {
                            if (level >= Levels.MirageDive &&
                                HasEffect(Buffs.DiveReady))
                                return MirageDive;
                        }
                    }
                    if (comboTime > 0)
                    {
                        if (lastComboMove == OriginalHook(DoomSpike) && level >= Levels.SonicThrust)
                            return SonicThrust;

                        if (lastComboMove is DRG.SonicThrust && level >= Levels.CoerthanTorment)
                            return CoerthanTorment;

                        if ((lastComboMove is DRG.DraconianFury))
                            return SonicThrust;
                    }

                    return OriginalHook(DoomSpike);
                }

                return actionID;
            }
        }

        internal class DragoonFangAndClawFeature : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DragoonFangAndClawFeature;

            protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
            {
                if (actionID is DRG.FangAndClaw)
                {
                    if (HasEffect(Buffs.EnhancedWheelingThrust) && level >= Levels.WheelingThrust)
                        return WheelingThrust;
                    if (HasEffect(Buffs.SharperFangAndClaw) && level >= Levels.FangAndClaw)
                        return FangAndClaw;

                }

                return actionID;
            }
        }
    }
}
