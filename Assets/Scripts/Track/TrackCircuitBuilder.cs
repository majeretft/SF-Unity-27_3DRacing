using UnityEngine;

namespace SF3DRacing
{
    public static class TrackCircuitBuilder
    {
        public static TrackPoint[] BuildCircuit(Transform transform, TrackTypeEnum type)
        {
            TrackPoint[] points = new TrackPoint[transform.childCount];

            ResetPoints(transform, points);
            LinkPoints(points, type);
            MarkPoints(type, points);
            MarkSceneDirty(transform, points);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(transform.GetComponent<TrackPointCircuit>());
            foreach (TrackPoint p in points)
            {
                UnityEditor.EditorUtility.SetDirty(p);
            }
#endif

            return points;
        }

        private static void MarkPoints(TrackTypeEnum type, TrackPoint[] points)
        {
            if (type == TrackTypeEnum.Circular)
            {
                points[0].IsLast = true;
            }

            if (type == TrackTypeEnum.Sprint)
            {
                points[^1].IsLast = true;
            }

            points[0].IsFirst = true;
        }

        private static void ResetPoints(Transform transform, TrackPoint[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = transform.GetChild(i).GetComponent<TrackPoint>();

                if (points[i] == null)
                {
                    Debug.LogError($"Child {i} doesn`t have TrackPoint component");
                    return;
                }

                points[i].Reset();
            }
        }

        private static void LinkPoints(TrackPoint[] points, TrackTypeEnum type)
        {
            for (int i = 0; i < points.Length - 1; i++)
            {
                points[i].Next = points[i + 1];
            }

            if (type == TrackTypeEnum.Circular)
            {
                points[^1].Next = points[0];
            }
        }

#if UNITY_EDITOR
        private static void MarkSceneDirty(Transform transform, TrackPoint[] points)
        {
            UnityEditor.EditorUtility.SetDirty(transform.GetComponent<TrackPointCircuit>());

            foreach (TrackPoint p in points)
            {
                UnityEditor.EditorUtility.SetDirty(p);
            }
        }
    }
#endif
}
