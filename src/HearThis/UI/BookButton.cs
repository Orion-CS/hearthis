using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using HearThis.Script;

namespace HearThis.UI
{
	public partial class BookButton : UserControl
	{
		private readonly BookInfo _model;
		private bool _selected;
		private SolidBrush _highlightBoxBrush;
		private SolidBrush _brushIfTranslated;
		private SolidBrush _brushIfNotTranslated;

		public BookButton(BookInfo model)
		{
			_model = model;
			InitializeComponent();
			int kMaxChapters = 150;//psalms
			Width = (int) (Width + ((double)model.ChapterCount / (double)kMaxChapters) * 25.0);

			_brushIfTranslated= new SolidBrush(AppPallette.DarkGray);
			_brushIfNotTranslated = new SolidBrush(Color.WhiteSmoke);

			_highlightBoxBrush = new SolidBrush(AppPallette.Orange);

			  //We'r'e doing ThreadPool instead of the more convenient BackgroundWorker based on experimentation and the advice on the web; we are doing relatively a lot of little threads here,
			//that don't really have to interact much with the UI until they are complete.
//            var waitCallback = new WaitCallback(GetStatsInBackground);
//            _percentageTranslated = 0;
//            ThreadPool.QueueUserWorkItem(waitCallback, this);
		}


//        static void GetStatsInBackground(object stateInfo)
//        {
//
//           BookButton button = stateInfo as BookButton;
//            button._percentageRecorded = button._bookInfo.CalculatePercentageRecorded();
//            button._percentageTranslated = button.ChapterInfo.CalculatePercentageTranslated();
//            lock (button)
//            {
//                if (button.IsHandleCreated && !button.IsDisposed)
//                {
//                    button.Invoke(new Action(delegate { button.Invalidate(); }));
//                }
//            }
//        }

		public bool Selected
		{
			get { return _selected; }
			set
			{
				if (_selected != value)
				{
					_selected = value;
					Invalidate();
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Brush fillBrush;
			if (_model.CalculatePercentageTranslated() > 0)
				fillBrush = _brushIfTranslated;
			else
			{
				fillBrush = _brushIfNotTranslated;
			}

			if (Selected)
			{
				e.Graphics.FillRectangle(_highlightBoxBrush, 0, 0, Width, Height);
			}
			else //this just shows up as a shadow
			{
				e.Graphics.FillRectangle(Brushes.Gray, 4, 4, Width - 5, Height - 5);
			}

			var r = new Rectangle(2, 3, Width - 4, Height - 5);
			e.Graphics.FillRectangle(fillBrush, r);

			//
			int greyWidth = Width - 4;
			int greyHeight = Height - 5;
			//             var r = new Rectangle(2, 3, greyWidth, greyHeight);
			//             var shadow = new Rectangle(r.Left, r.Top, r.Width, r.Height);
			//             shadow.Offset(1, 1);
			//             if (Selected)
			//             {
			//                 e.Graphics.FillRectangle(_highlightBoxBrush, 0, 0, Width, Height);
			//             }
			//             else
			//             {
			//                 e.Graphics.FillRectangle(Brushes.Gray, shadow);
			//             }
			//             using (Brush _fillBrush = new SolidBrush(_percentageTranslated > 0 ? AppPallette.DarkGray : Color.WhiteSmoke))
			//             {
			//                 e.Graphics.FillRectangle(_fillBrush, r);
			//             }
			int percentRecorded = _model.CalculatePercentageRecorded();
			if (percentRecorded > 0)
			{
				int recordedWidth = Math.Max(2, (int) (greyWidth/(100.0/(float) percentRecorded)));
				r = new Rectangle(2, 3, recordedWidth, greyHeight);
				e.Graphics.FillRectangle(AppPallette.BlueBrush, r);
			}

		}

	}
}
