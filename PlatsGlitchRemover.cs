using ModCommon.Util;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace HK.RadiancePlatsGlitchRemover {
    public class PlatsGlitchRemover: MonoBehaviour {
        private PlayMakerFSM fsm;
        private bool lastAttackWasSwordWalls;
        public void Awake() {
            On.PlayMakerFSM.OnEnable += OnEnableFSM;
        }

        public void OnEnableFSM(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
            orig(self);

            if (self.gameObject.name == "Absolute Radiance") {
                if (self.FsmName == "Attack Choices") {
                    fsm = self;

                    self.InsertAction("L or R Choice", new CallMethod {
                        behaviour = this,
                        methodName = "SwordWallAttack",
                        parameters = new FsmVar[0],
                        everyFrame = false
                    }, 0);
                    self.AddAction("Eye Beam Wait", new CallMethod {
                        behaviour = this,
                        methodName = "NonSwordWallAttack",
                        parameters = new FsmVar[0],
                        everyFrame = false
                    });
                    self.AddAction("Beam Sweep L 2", new CallMethod {
                        behaviour = this,
                        methodName = "NonSwordWallAttack",
                        parameters = new FsmVar[0],
                        everyFrame = false
                    });
                    self.AddAction("Beam Sweep R 2", new CallMethod {
                        behaviour = this,
                        methodName = "NonSwordWallAttack",
                        parameters = new FsmVar[0],
                        everyFrame = false
                    });
                    self.AddAction("Nail Fan Wait", new CallMethod {
                        behaviour = this,
                        methodName = "NonSwordWallAttack",
                        parameters = new FsmVar[0],
                        everyFrame = false
                    });
                    self.AddAction("Orb Wait", new CallMethod {
                        behaviour = this,
                        methodName = "NonSwordWallAttack",
                        parameters = new FsmVar[0],
                        everyFrame = false
                    });
                }

                if (self.FsmName == "Control") {
                    self.AddAction("Stun1 Roar", new CallMethod {
                        behaviour = this,
                        methodName = "PreventStuckGlitch",
                        parameters = new FsmVar[0],
                        everyFrame = false
                    });
                }
            }
        }

        public void SwordWallAttack() {
            if (lastAttackWasSwordWalls && fsm != null) {
                fsm.SetState("A2 Choice");
            } else {
                lastAttackWasSwordWalls = true;
            }
        }

        public void NonSwordWallAttack() {
            lastAttackWasSwordWalls = false;
        }

        public void PreventStuckGlitch() {
            GameObject.Find("Absolute Radiance").LocateMyFSM("Control").FsmVariables.GetFsmBool("Please Cast").Value = false;
        }
    }
}