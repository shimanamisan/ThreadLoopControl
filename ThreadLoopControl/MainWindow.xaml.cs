using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading; // 追加

namespace ThreadLoopControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= 100; i++)
            {
                System.Threading.Thread.Sleep(200);
                textBlock.Text = Convert.ToString($"00:{i}");
                DoEvents();
            }
        }

        private void DoEvents()
        {
            // https://www.ipentec.com/document/csharp-wpf-implement-application-doevents
            // DispatcherFrameオブジェクトが、アプリケーションで保留中の作業項目を処理するループオブジェクトである
            // BeginInvokeメソッドで、与えたデリゲートを指定した優先順位で非同期に実行される
            DispatcherFrame frame = new DispatcherFrame();

            // 処理されるデリゲートがDispatcherOperationCallbackオブジェクトになる
            // デリゲートに渡すオブジェクトがDispatcherFrame オブジェクトになりる
            var callback = new DispatcherOperationCallback(ExitFrames);

            // DispatcherPriorityでBackgroundを与えているため、他のすべての非アイドル状態の処理が完了した後に操作が処理される
            // アイドル状態：何も処理を行っておらず、すぐ実行できる状態のこと
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, callback, frame);

            // PushFrameメソッドで、PushFrame メソッドの引数に与えたDispatcherFrameオブジェクトのループに入る
            // ループの反復ごとに、ExitFramesコールバック関数が実行される
            // コールバック関数で、Continueプロパティの値を確認してループの継続を判定する
            // 今回のコードの場合は常にfalseとなるため、ループは常に終了する
            Dispatcher.PushFrame(frame);
        }

        /*
        // 下記のコードでも可
        private void DoEvents()
        {
          DispatcherFrame frame = new DispatcherFrame();

          Func<object, object> cfunc = new Func<object, object>(procd);

          var callback = new DispatcherOperationCallback(ExitFrames);
          Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, callback, frame);
          Dispatcher.PushFrame(frame);
        }
        */

        private object ExitFrames(object obj)
        {
            ((DispatcherFrame)obj).Continue = false;
            return null;
        }

        /*
        // または下記のコードでも可
        private void DoEvents()
        {
          DispatcherFrame frame = new DispatcherFrame();
          var callback = new DispatcherOperationCallback(obj =>
          {
            ((DispatcherFrame)obj).Continue = false;
            return null;
          });
          Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, callback, frame);
          Dispatcher.PushFrame(frame);
        }
        */
    }
}
