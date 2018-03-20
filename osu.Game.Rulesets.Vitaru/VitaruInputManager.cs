﻿using eden.Game.GamePieces;
using osu.Framework.Configuration;
using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Game.Rulesets.Vitaru.UI;
using Symcol.Rulesets.Core;

namespace osu.Game.Rulesets.Vitaru
{
    public class VitaruInputManager : SymcolInputManager<VitaruAction>
    {
        private Bindable<bool> debugUI = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.DebugOverlay);
        private Bindable<bool> comboFire = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.ComboFire);

        protected override bool VectorVideo => VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.VectorVideos);

        public VitaruInputManager(RulesetInfo ruleset, int variant) : base(ruleset, variant, SimultaneousBindingMode.Unique)
        {
            if (debugUI)
                Add(new DebugValueUI { Anchor = Framework.Graphics.Anchor.CentreLeft, Origin = Framework.Graphics.Anchor.CentreLeft, Position = new OpenTK.Vector2(10, 0)});
            if (comboFire)
                Add(new ComboFire());
        }
    }

    public enum VitaruAction
    {
        None = -1,

        //Movement
        Left = 0,
        Right,
        Up,
        Down,

        //Self-explaitory
        Shoot,
        Spell,

        //Slows the player + reveals hitbox
        Slow,
        Fast,

        //Sakuya
        Increase,
        Decrease,

        //Kokoro
        RightShoot,
        LeftShoot,

        //Nue
        Spell2,
        Spell3,
        Spell4
    }
}
