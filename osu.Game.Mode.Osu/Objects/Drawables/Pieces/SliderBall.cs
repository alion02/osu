﻿//Copyright (c) 2007-2016 ppy Pty Ltd <contact@ppy.sh>.
//Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Transformations;
using osu.Framework.Input;
using OpenTK.Graphics;

namespace osu.Game.Modes.Osu.Objects.Drawables.Pieces
{
    public class SliderBall : Container, ISliderProgress
    {
        private readonly Slider slider;
        private Box follow;

        public SliderBall(Slider slider)
        {
            this.slider = slider;
            Masking = true;
            AutoSizeAxes = Axes.Both;
            BlendingMode = BlendingMode.Additive;
            Origin = Anchor.Centre;

            Children = new Drawable[]
            {
                follow = new Box
                {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Colour = Color4.Orange,
                    Width = 64,
                    Height = 64,
                },
                new Container
                {
                    Masking = true,
                    AutoSizeAxes = Axes.Both,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Colour = Color4.Cyan,
                    CornerRadius = 32,
                    Children = new[]
                    {
                        new Box
                        {

                            Width = 64,
                            Height = 64,
                        },
                    }
                }

            };
        }

        private InputState lastState;

        protected override bool OnMouseDown(InputState state, MouseDownEventArgs args)
        {
            lastState = state;
            return base.OnMouseDown(state, args);
        }

        protected override bool OnMouseUp(InputState state, MouseUpEventArgs args)
        {
            lastState = state;
            return base.OnMouseUp(state, args);
        }

        protected override bool OnMouseMove(InputState state)
        {
            lastState = state;
            return base.OnMouseMove(state);
        }

        bool tracking;
        protected bool Tracking
        {
            get { return tracking; }
            set
            {
                if (value == tracking) return;

                tracking = value;

                follow.ScaleTo(tracking ? 2.4f : 1, 140, EasingTypes.Out);
                follow.FadeTo(tracking ? 0.8f : 0, 140, EasingTypes.Out);
            }
        }

        protected override void Update()
        {
            base.Update();

            CornerRadius = DrawWidth / 2;
            Tracking = lastState != null && Contains(lastState.Mouse.NativeState.Position) && lastState.Mouse.HasMainButtonPressed;
        }

        public void UpdateProgress(double progress, int repeat)
        {
            Alpha = Time.Current >= slider.StartTime && Time.Current <= slider.EndTime ? 1 : 0;
            Position = slider.Curve.PositionAt(progress);
        }
    }
}