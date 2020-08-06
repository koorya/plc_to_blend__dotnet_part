using System;
using System.Threading;
using System.Windows.Forms;

using OMRON.Compolet.CIPCompolet64;

using NetMQ;
using NetMQ.Sockets;

using System.Text;

namespace common_compolet_pure
{
    partial class Form1
    {		
        private bool cip_end;
		
        private void cip_update(){
            cip_end = true;
            try
            {
               
                using (var publisher = new PublisherSocket())
                {
                    publisher.Connect("tcp://127.0.0.1:5550");
                    while(cip_end)
                    {

                        object obj = this.commonCompolet1.ReadVariable("eth_var");
                        if (obj == null)
                        {
                            throw new NotSupportedException();
                        }


                    ///	VariableInfo info = null;

                    //  info = this.commonCompolet1.GetVariableInfo(varname);
                        string str = this.GetValueOfVariables(obj);

                        this.txtValue.Text = $"[{BitConverter.ToDouble((byte[])obj, 0)}], [{BitConverter.ToDouble((byte[])obj, 8)}],"+
											$"[{BitConverter.ToDouble((byte[])obj, 16)}], [{BitConverter.ToDouble((byte[])obj, 24)}]";//str;

                      //  byte[][] bytes = {Encoding.ASCII.GetBytes("foo"), Encoding.ASCII.GetBytes($"{x}")};
					  	
                  //      publisher.SendMultipartBytes(bytes);
                         //   .SendMoreFrame("foo") // Topic
                          //  .SendFrame($"{x}"); // Message
						publisher.SendFrame((byte[])obj);
                        Thread.Sleep(50);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }
		
        private object RemoveBrackets(string val)
		{
			object obj = string.Empty;
			if (val.IndexOf("[") >= 0)
			{
				string str = val.Trim('[', ']');
				str = str.Replace("][", ",");
				obj = str.Split(',');
			}
			else
			{
				obj = val;
			}
			return obj;
		}
		private byte[] ObjectToByteArray(object obj)
		{
			if(obj is Array)
			{
				Array arr = obj as Array;
				Byte[] bin = new Byte[arr.Length];
				for(int i = 0 ; i < bin.Length ; i++)
				{
					bin[i] = Convert.ToByte(arr.GetValue(i));
				}
				return bin;
			}
			else
			{
				return new Byte[1]{ Convert.ToByte(obj) };
			}
		}
        private string GetValueOfVariables(object val)
		{
			string valStr = string.Empty;
			if (val.GetType().IsArray)
			{
				Array valArray = val as Array;
				if (valArray.Rank == 1)
				{
					valStr += "[";
					foreach (object a in valArray)
					{
						valStr += this.GetValueString(a) + ",";
					}
					valStr = valStr.TrimEnd(',');
					valStr += "]";
				}
				else if (valArray.Rank == 2)
				{
					for (int i = 0; i <= valArray.GetUpperBound(0); i++)
					{
						valStr += "[";
						for (int j = 0; j <= valArray.GetUpperBound(1); j++)
						{
							valStr += this.GetValueString(valArray.GetValue(i, j)) + ",";
						}
						valStr = valStr.TrimEnd(',');
						valStr += "]";
					}
				}
				else if (valArray.Rank == 3)
				{
					for (int i = 0; i <= valArray.GetUpperBound(0); i++)
					{
						for (int j = 0; j <= valArray.GetUpperBound(1); j++)
						{
							valStr += "[";
							for (int z = 0; z <= valArray.GetUpperBound(2); z++)
							{
								valStr += this.GetValueString(valArray.GetValue(i, j, z)) + ",";
							}
							valStr = valStr.TrimEnd(',');
							valStr += "]";
						}
					}
				}
			}
			else
			{
				valStr = this.GetValueString(val);
			}
			return valStr;
		}
        private string GetValueString(object val)
		{
			if (val is float || val is double)
			{
				return string.Format("{0:R}", val);
			}
			else
			{
				return val.ToString();
			}
		}
    }
}