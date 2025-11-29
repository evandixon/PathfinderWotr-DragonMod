using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.QA.Statistics;
using Kingmaker.UnitLogic;

namespace DragonMod.Content
{
    public class MinimumXPComponent : UnitFactComponentDelegate
    {
        public int MinimumXP { get; set; }

        private bool _applied;

        public override void OnTurnOn()
        {
            if (_applied) return;

            var progression = Owner.Progression;

            if (progression.Experience < MinimumXP)
            {
                progression.Experience = MinimumXP;
                EventBus.RaiseEvent(delegate (IUnitGainExperienceHandler h)
                {
                    h.HandleUnitGainExperience(Owner, MinimumXP, ExperienceGainStatistic.GainType.None);
                });
            }

            _applied = true;
        }
    }
}
