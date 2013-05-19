using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SchemeEditor.Schema
{
    public class Room : IJsonObject
    {
        public Room()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Title { get; set; }
        public string Id { get; private set; }
        public Color Color { get; set; }
        public Point[] Points { get; set; }

        public List<Wall> GetWalls()
        {
            var res = new List<Wall>();
            Point prev = new Point();
            bool door = false;
            foreach (var p in Points)
            {
                Point cur = p;
                if (prev.X == 0 && prev.Y == 0)
                {
                    prev = p;
                }
                else if (cur.X < 0 && cur.Y < 0)
                {
                    door = true;
                }
                else
                {
                    if (!door)
                    {
                        var wall = new Wall();
                        wall.Start = prev;
                        wall.End = cur;
                        res.Add(wall);
                    }
                    prev = cur;
                    door = false;
                }
            }

            return res;
        }

        public void SetWalls(IEnumerable<Wall> walls)
        {
            if (walls.Skip(1).Any())
            {
                var traversed = walls.ToList();
                var points= new List<Point>();
                var first = traversed.First();
                traversed.RemoveAt(0);
                points.Add(first.Start);
                if (first.IsDoor)
                {
                    points.Add(new Point(-1, -1));
                }
                points.Add(first.End);
                var current = first.End;
                while (traversed.Any())
                {
                    var index = traversed.FindIndex(w => w.Start == current);
                    if (index >= 0)
                    {
                        var wall = traversed[index];
                        traversed.RemoveAt(index);
                        if (wall.IsDoor)
                        {
                            points.Add(new Point(-1, -1));
                        }
                        points.Add(wall.End);
                        current = wall.End;
                    }
                    else
                    {
                        index = traversed.FindIndex(w => w.End == current);
                        if (index >= 0)
                        {
                            var wall = traversed[index];
                            traversed.RemoveAt(index);
                            if (wall.IsDoor)
                            {
                                points.Add(new Point(-1, -1));
                            }
                            points.Add(wall.Start);
                            current = wall.Start;
                        }
                        else
                        {
                            points.Add(new Point(-1, -1));

                            var minDist = double.MaxValue;
                            var minIndex = -1;
                            bool isStart = false;
                            for (int i = 0; i < traversed.Count; i++)
                            {
                                var startDist = Utils.GetPointsDistance(traversed[i].Start, current);
                                var endDist = Utils.GetPointsDistance(traversed[i].End, current);
                                if (startDist < minDist)
                                {
                                    minDist = startDist;
                                    minIndex = i;
                                    isStart = true;
                                }
                                if (endDist < minDist)
                                {
                                    minDist = endDist;
                                    minIndex = i;
                                    isStart = false;
                                }
                            }

                            var wall = traversed[minIndex];
                            traversed.RemoveAt(minIndex);
                            if (isStart)
                            {
                                points.Add(wall.Start);
                                points.Add(wall.End);
                                current = wall.End;
                            }
                            else
                            {
                                points.Add(wall.End);
                                points.Add(wall.Start);
                                current = wall.Start;
                            }
                        }
                    }
                }

                if (current != points.First())
                {
                    points.Add(new Point(-1, -1));
                    points.Add(points.First());
                }

                Points = points.ToArray();
            }
        }

        public string GetHexColor()
        {
            return "#" + Color.R.ToString("X2") + Color.G.ToString("X2") + Color.B.ToString("X2");
        }

        public Dictionary<string, object> ToJsonObject()
        {
            return new Dictionary<string, object>
            {
                { "title", Title },
                { "id", Id },
                { "color", GetHexColor() },
                { "points", Points.Select(p => new[] { p.X, p.Y }) }
            };
        }


        public void FillWithJsonObject(JContainer dict, Scheme scheme)
        {
            Title = (string)dict["title"];
            Id = (string)dict["id"];
            Color = (Color)ColorConverter.ConvertFromString((string)dict["color"]);
            Points = (dict["points"]).Select(p => new Point((double)p[0], (double)p[1])).ToArray();
        }
    }
}
