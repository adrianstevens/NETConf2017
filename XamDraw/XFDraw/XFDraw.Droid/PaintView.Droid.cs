using System;
using Android.Views;
using Android.Graphics;
using Android.Content;
using Android.Util;
using System.Collections.Generic;

namespace XFDraw.Droid
{
	public class PaintView : View
	{
        public event EventHandler LineDrawn;

        Dictionary<int, MotionEvent.PointerCoords> coords = new Dictionary<int, MotionEvent.PointerCoords>();

		Canvas drawCanvas;
		Bitmap canvasBitmap;
        Paint paint;

		public PaintView(Context context): base(context, null, 0)
        {
            paint = new Paint() { Color = Color.Blue, StrokeWidth = 5f, AntiAlias = true };
            paint.SetStyle(Paint.Style.Stroke);
        }
		public PaintView(Context context, IAttributeSet attrs) : base(context, attrs) { }
		public PaintView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) { }

		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged(w, h, oldw, oldh);

			canvasBitmap = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888); // full-screen bitmap

			drawCanvas = new Canvas(canvasBitmap); // the canvas will draw into the bitmap
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			switch (e.ActionMasked)
			{
				case MotionEventActions.Down:
				{
					int id = e.GetPointerId(0);

	   			    var start = new MotionEvent.PointerCoords();
					e.GetPointerCoords(id, start);
					coords.Add(id, start);

					return true;
				}

				case MotionEventActions.PointerDown:
				{
					int id = e.GetPointerId(e.ActionIndex);

					var start = new MotionEvent.PointerCoords();
					e.GetPointerCoords(id, start);
					coords.Add(id, start);

					return true;
				}
				
				case MotionEventActions.Move:
				{
					for (int index = 0; index < e.PointerCount; index++) 
					{
						var id = e.GetPointerId(index);

						float x = e.GetX(index);
						float y = e.GetY(index);

						drawCanvas.DrawLine(coords[id].X, coords[id].Y, x, y, paint);
						
						coords[id].X = x;
						coords[id].Y = y;

                        if (LineDrawn != null)
                            LineDrawn(this, EventArgs.Empty);
                    }

					Invalidate();

					return true;
				}
				
				case MotionEventActions.PointerUp:
				{
					int id = e.GetPointerId(e.ActionIndex);
					coords.Remove(id);
					return true;
				}
					
				case MotionEventActions.Up:
				{
					int id = e.GetPointerId(0);
					coords.Remove(id);
					return true;
				}

				default:
					return false;
			}
		}

		protected override void OnDraw(Canvas canvas)
		{
			// Copy the off-screen canvas data onto the View from it's associated Bitmap (which stores the actual drawn data)
			canvas.DrawBitmap(canvasBitmap, 0, 0, null);
		}

		public void Clear()
		{
			drawCanvas.DrawColor(Color.Black, PorterDuff.Mode.Clear); // Paint the off-screen buffer black

			Invalidate(); // Call Invalidate to redraw the view
		}

        public void SetInkColor (Color color)
        {
           paint.Color = color;
        }
	}
}