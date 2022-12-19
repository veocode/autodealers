using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.Native;


namespace VeoAutoMod.Objects
{
    enum MarkerState
    {
        Idle,
        Touched,
        Deleted
    }

    enum MarkerCheckpointType
    {
        Traditional = 0,
        SmallArrow = 5,
        DoubleArrow = 6,
        TripleArrow = 7,
        CycleArrow = 8,
        ArrowInCircle = 10,
        DoubleArrowInCircle = 11,
        TripleArrowInCircle = 12,
        CycleArrowInCircle = 13,
        CheckerInCircle = 14,
        Arrow = 15
    }

    static class MarkerManager
    {
        private static List<Marker> markers = new List<Marker>();

        public static Marker CreateMarker(Vector3 position, float radius = 1f, float height = 0.3f, BlipColor color = BlipColor.Yellow, bool isBlip = false, bool isRoute = false)
        {
            Marker marker = new Marker(position, radius, height, color, isBlip, isRoute);

            markers.Add(marker);

            return marker;
        }

        public static void Update()
        {
            List<Marker> removeList = new List<Marker>();

            foreach (Marker marker in markers)
            {
                marker.Update();
                if (marker.IsDeleted()) removeList.Add(marker);
            }

            foreach (Marker marker in removeList)
            {
                markers.Remove(marker);
            }
        }
    }

    class Marker
    {
        private int handle;
        private MarkerState state = MarkerState.Idle;

        private Vector3 position;
        private bool isBlip = false;
        private Blip blip;

        private float touchRadius;

        private Action<Marker> touchCallback;
        private Action<Marker> leaveCallback;

        public Marker(Vector3 position, float radius = 1f, float height = 1f, BlipColor color = BlipColor.Yellow, bool isBlip = false, bool isRoute = false)
        {

            this.position = position;
            this.isBlip = isBlip;
            
            touchRadius = 1.2f;

            int r = 255, g = 0, b = 0;

            switch (color)
            {
                case BlipColor.Blue:
                    r = 0; g = 0; b = 255;
                    break;
                case BlipColor.Green:
                    r = 0; g = 255; b = 0;
                    break;
                case BlipColor.Red:
                    r = 255; g = 0; b = 0;
                    break;
                case BlipColor.White:
                    r = 255; g = 255; b = 255;
                    break;
                case BlipColor.Yellow:
                    r = 255; g = 255; b = 0;
                    break;
            }

            handle = Function.Call<int>(Hash.CREATE_CHECKPOINT,
               0,
               position.X,
               position.Y,
               position.Z - 1f,
               position.X,
               position.Y,
               0f,
               radius,
               r,
               g,
               b,
               100,
               0);

            Function.Call(Hash._SET_CHECKPOINT_ICON_RGBA, handle, 0, 0, 0, 0);

            if (isBlip)
            {
                blip = GTA.World.CreateBlip(position);
                blip.Color = color;
                blip.Scale = 1f;
                blip.ShowRoute = isRoute;
                blip.IsShortRange = true;
            }

            SetHeight(height + 1f);
        }

        public void SetState(MarkerState state) => this.state = state;
        public MarkerState GetState() => state;
        public bool Is(MarkerState inState) => state == inState;
        public bool IsIdle() => state == MarkerState.Idle;
        public bool IsTouched() => state == MarkerState.Touched;
        public bool IsDeleted() => state == MarkerState.Deleted;

        public void SetHeight(float height) => Function.Call(Hash.SET_CHECKPOINT_CYLINDER_HEIGHT, handle, 1, height, 1);

        public void SetTouchRadius(float radius) => touchRadius = radius;
        public void OnPlayerTouch(Action<Marker> callback) => touchCallback = callback;
        public void OnPlayerLeft(Action<Marker> callback) => leaveCallback = callback;

        public void Update()
        {
            if (IsDeleted()) { return; }

            if (IsIdle() && position.DistanceTo(Game.Player.Character.Position) <= touchRadius)
            {
                SetState(MarkerState.Touched);
                touchCallback?.Invoke(this);
            }

            if (IsTouched() && position.DistanceTo(Game.Player.Character.Position) > touchRadius)
            {
                SetState(MarkerState.Idle);
                leaveCallback?.Invoke(this);
            }
        }

        public void Delete()
        {
            Function.Call(Hash.DELETE_CHECKPOINT, handle);

            if (isBlip)
            {
                blip.Delete();
                blip = null;
            }

            SetState(MarkerState.Deleted);
        }
    }
}

