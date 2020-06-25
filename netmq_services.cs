using System;
using NetMQ;
using NetMQ.Sockets;
using System.Threading;
using System.Windows.Forms;


using System.Text;

namespace common_compolet_pure
{
    
    partial class Form1
    {		
        private double time_cnt;
        private bool mq_end;
        private void mq_test()
        {
            
            mq_end = true;
			time_cnt = 0;
            try
            {
                using (var publisher = new PublisherSocket())
                {
                    publisher.Connect("tcp://127.0.0.1:5550");
                    while(mq_end)
                    {

                        int x = (int)(100*Math.Sin(time_cnt));
                        int y = 0;//(int)obj1;
                        this.chkPx.Location = new System.Drawing.Point(x + 150, 257 + y);

                        byte[][] bytes = {Encoding.ASCII.GetBytes("foo"), Encoding.ASCII.GetBytes($"{x}")};

                        publisher.SendMultipartBytes(bytes);
                        time_cnt += 0.05;

                        Thread.Sleep(10);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }
    }
}