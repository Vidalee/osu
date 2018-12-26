// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Graphics.UserInterface;
using osu.Game.Online.Multiplayer;
using osu.Game.Screens.Multi;
using osu.Game.Screens.Multi.Lounge.Components;
using osu.Game.Screens.Multi.Match.Components;

namespace osu.Game.Tests.Visual
{
    public class TestCaseMatchSettingsOverlay : OsuTestCase
    {
        public override IReadOnlyList<Type> RequiredTypes => new[]
        {
            typeof(MatchSettingsOverlay)
        };

        [Cached(Type = typeof(IRoomManager))]
        private TestRoomManager roomManager = new TestRoomManager();

        private Room room;
        private TestRoomSettings settings;

        [SetUp]
        public void Setup() => Schedule(() =>
        {
            room = new Room();
            settings = new TestRoomSettings(room)
            {
                RelativeSizeAxes = Axes.Both,
                State = Visibility.Visible
            };

            Child = settings;
        });

        [Test]
        public void TestButtonEnabledOnlyWithNameAndBeatmap()
        {
            AddStep("clear name and beatmap", () =>
            {
                room.Name.Value = "";
                room.Playlist.Clear();
            });

            AddAssert("button disabled", () => !settings.ApplyButton.Enabled);

            AddStep("set name", () => room.Name.Value = "Room name");
            AddAssert("button disabled", () => !settings.ApplyButton.Enabled);

            AddStep("set beatmap", () => room.Playlist.Add(new PlaylistItem { Beatmap = new DummyWorkingBeatmap().BeatmapInfo }));
            AddAssert("button enabled", () => settings.ApplyButton.Enabled);

            AddStep("clear name", () => room.Name.Value = "");
            AddAssert("button disabled", () => !settings.ApplyButton.Enabled);
        }

        [Test]
        public void TestCorrectSettingsApplied()
        {
            const string expected_name = "expected name";
            TimeSpan expectedDuration = TimeSpan.FromMinutes(15);

            Room createdRoom = null;

            AddStep("setup", () =>
            {
                settings.NameField.Current.Value = expected_name;
                settings.DurationField.Current.Value = expectedDuration;

                roomManager.CreateRequested = r => createdRoom = r;
            });

            AddStep("create room", () => settings.ApplyButton.Action.Invoke());
            AddAssert("has correct name", () => createdRoom.Name.Value == expected_name);
            AddAssert("has correct duration", () => createdRoom.Duration.Value == expectedDuration);
        }

        private class TestRoomSettings : MatchSettingsOverlay
        {
            public new TriangleButton ApplyButton => base.ApplyButton;

            public new OsuTextBox NameField => base.NameField;
            public new OsuDropdown<TimeSpan> DurationField => base.DurationField;

            public TestRoomSettings(Room room)
                : base(room)
            {
            }
        }

        private class TestRoomManager : IRoomManager
        {
            public Action<Room> CreateRequested;

            public event Action<Room> OpenRequested;

            public IBindableCollection<Room> Rooms { get; } = null;

            public void CreateRoom(Room room) => CreateRequested?.Invoke(room);

            public void JoinRoom(Room room) => throw new NotImplementedException();

            public void PartRoom() => throw new NotImplementedException();

            public void Filter(FilterCriteria criteria) => throw new NotImplementedException();
        }
    }
}
