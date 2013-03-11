using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;


namespace THOK.Zeng.ComfixtureHandle.el103
{
    public class ELabelOperator:THOK.Zeng.ComfixtureHandle.ComfixtureHandle, THOK.Zeng.ComfixtureHandle.IELabelOperator
    {
        /// <summary>
        /// 设置字体颜色
        /// </summary>
        /// <param name="color"></param>
        public void SetShowColor(TextColor color)
        {
            ((Protocal)this.Com.Encoder).BtShowColor = (byte)color;
        }
        /// <summary>
        /// 设置标签显示类型
        /// </summary>
        /// <param name="showModel">ShowDevice指示设备,TwoBitShowDevice两位数显</param>
        public void SetFunctionType(ShowModel showModel)
        {
            ((Protocal)this.Com.Encoder).FormatType = (byte)showModel;
        }
        /// <summary>
        /// 设置闪烁状态
        /// </summary>
        /// <param name="flashState">不变,正常闪烁,快速闪烁</param>
        /// <param name="flashModel">CONFIRM熄灭返回按键,当递减到“00000”按CONFIRM熄灭,按CONFIRM灯闪烁5次后熄灭返回当前数值</param>
        /// <param name="flashSwitchBit"></param>
        public void SetFlashState(FlashState flashState, FlashModel flashModel, int flashSwitchBit)
        {
            byte[] switchBit = BitConverter.GetBytes(flashSwitchBit);
            ((Protocal)this.Com.Encoder).BtFlashState[0] = switchBit[0];
            ((Protocal)this.Com.Encoder).BtFlashState[1] = (byte)((switchBit[1] & 0X3FFF) ^ (byte)((byte)flashState << 6));
            ((Protocal)this.Com.Encoder).BtFlashState[2] = (byte)flashModel;
        }
        /// <summary>
        /// 设置背景灯光，指示灯，蜂鸣声音
        /// </summary>
        /// <param name="useBkLight">保留字段</param>
        /// <param name="lightFrequency">保留字段频率</param>
        /// <param name="useInstructions">指示灯</param>
        /// <param name="instructionsFrequency">指示灯闪烁频率</param>
        /// <param name="useSinging">蜂鸣声</param>
        /// <param name="singFrequency">蜂鸣声响音频率</param>
        public void SetFunction(FuntionState useBkLight, byte lightFrequency, FuntionState useInstructions,
            byte instructionsFrequency, FuntionState useSinging, byte singFrequency)
        {
            ((Protocal)this.Com.Encoder).BtFunction[0] = (byte)((byte)useBkLight * 64 | lightFrequency);
            ((Protocal)this.Com.Encoder).BtFunction[1] = (byte)((byte)useInstructions * 64 | instructionsFrequency);
            ((Protocal)this.Com.Encoder).BtFunction[2] = (byte)((byte)useSinging * 64 | singFrequency);
        }

        /// <summary>
        /// 按键状态及频率设置
        /// </summary>
        /// <param name="state1">按键1状态</param>
        /// <param name="frequency1">按键1闪烁频率</param>
        /// <param name="state2">按键2状态</param>
        /// <param name="frequency2">按键2闪烁频率</param>
        /// <param name="state3">按键3状态</param>
        /// <param name="frequency3">按键3闪烁频率</param>
        public void SetKeysState(FuntionState state1, int frequency1, FuntionState state2, int frequency2, FuntionState state3, int frequency3)
        {
            ((Protocal)this.Com.Encoder).BtKeyLightState[0] = (byte)((byte)state1 * 64 | frequency1);
            ((Protocal)this.Com.Encoder).BtKeyLightState[1] = (byte)((byte)state2 * 64 | frequency2);
            ((Protocal)this.Com.Encoder).BtKeyLightState[2] = (byte)((byte)state3 * 64 | frequency3);
        }

        /// <summary>
        /// 操作电子标签
        /// </summary>
        /// <param name="address">电子标签地址</param>
        /// <param name="cmdType">命令类型</param>
        private void ExecuteCmd(int address, CmdType cmdType ,object data)
        {
            CmdData cmddata = new CmdData();
            cmddata.address = address;
            cmddata.cmdType = cmdType;
            cmddata.data = data;
            this.Send(cmddata);
        }

        /// <summary>
        /// 发送数据到电子标签,标签时时监控
        /// </summary>
        /// <param name="address">电子标签地址</param>
        /// <param name="data">数据</param>
        public void SendData(int address, string data)
        {
            ExecuteCmd(address, CmdType.SendData, data);
        }
        
        /// <summary>
        /// 发送数据到电子标签
        /// </summary>
        /// <param name="address">电子标签地址</param>
        /// <param name="data">数据</param>
        public void ShowData(int address, string data)
        {
            ExecuteCmd(address, CmdType.ShowOnly, data);
        }
        /// <summary>
        /// 喇叭
        /// </summary>
        /// <param name="address">电子标签地址</param>
        public void LoudSpeaker(int address)
        {
            byte[] btData = new byte[2];
            btData[1] = 1;
            ExecuteCmd(address, CmdType.LoudSpeaker, btData);
        }
        /// <summary>
        /// 关灯
        /// </summary>
        /// <param name="address">电子标签地址</param>
        public void CloseLight(int address)
        {
            byte[] btData = new byte[2];
            ExecuteCmd(address, CmdType.CloseLight, btData);
        }
        /// <summary>
        /// 开灯
        /// </summary>
        /// <param name="address">电子标签地址</param>
        public void OpenLight(int address)
        {
            byte[] btData = new byte[2];
            btData[1] = 1;
            ExecuteCmd(address, CmdType.OpenLight, btData);
        }
        /// <summary>
        /// 复位
        /// </summary>
        /// <param name="address">电子标签地址</param>
        public void ResetElectronicLabel(int address)
        {
            byte[] btData = new byte[2];
            ExecuteCmd(address, CmdType.ResetElectronicLabel, btData);
        }
        /// <summary>
        /// 控制板ID
        /// </summary>
        /// <param name="address">电子标签地址</param>
        public void ShowControlPlateID(int id)
        {
            byte[] btData = new byte[2];
            btData[1] = (byte)(id % 256);
            ExecuteCmd(0, CmdType.ControlPlateID, btData);
        }
        public void ResponseKeyAck()
        {
            byte[] btData = new byte[2];
            //btData[0] = (byte)(address % 256);

            CmdData cmddata = new CmdData();
            cmddata.address = 0;
            cmddata.cmdType = CmdType.KeyRaisedAck;
            cmddata.data = btData;

            this.Com.Write(new SendDataPacket() { data = cmddata, btdata = this.Com.Encoder.Encode(cmddata) });
        }
        public void ResetCotrolPlate()
        {
            byte[] btData = new byte[2];
            ExecuteCmd(0, CmdType.ResetControlPlate, btData);
        }

        public override void com_Data_Return(object Data)
        {
            PacketRecv pr = (PacketRecv)Data;
            if (pr.recvType != RecvType.Correct && Ack == null)
            {
                return;
            }
            if (pr == null)
                return;
            string recv = "";

            switch (pr.recvType)
            {
                case RecvType.Correct:
                    this.CurrentData = null;
                    break;
                case RecvType.CheckError:
                    Ack(pr.address, "CheckError", "err");
                    break;
                case RecvType.Timeout:
                    Ack(pr.address, "Timeout", "err");
                    break;
                case RecvType.Others:
                    Ack(pr.address, "控制卡其他不可识别错误！", "err");
                    break;
                case RecvType.NotExist:
                    Ack(pr.address, "NotExist", "err");
                    break;
                case RecvType.KeyEvent:
                    recv = pr.recv.Substring(14, pr.recv.Length - 18);
                    this.ResponseKeyAck();
                    for (int j = 0; j < recv.Length / 16; j++)
                    {
                        int address = int.Parse(recv.Substring(j * 16, 2), System.Globalization.NumberStyles.HexNumber);
                        string data = recv.Substring(j * 16 + 2, 14);
                        OnKeyRaised(address, data);
                    }
                    break;
                case RecvType.EndError:
                    Ack(pr.address, "EndError", "err");
                    break;
                case RecvType.SRamFull:
                    Ack(pr.address, "SRamFull！", "err");
                    break;
                case RecvType.DeleteTable:
                    //this.DeleteData(pr.address);
                    Ack(pr.address, "DeleteTable！", "err");
                    break;
                case RecvType.ResetCorrect:
                    Ack(pr.address, "控制卡复位成功！", "success");
                    break;
                case RecvType.ResetError:
                    Ack(pr.address, "控制卡复位失败！", "err");
                    break;
                default:
                    break;
        
            }  
        }
        private void OnKeyRaised(int address, string recv)
        {
            if (Ack != null)
            {
                //do something others

                int nLen = recv.Length / 2;
                int keyNo = 0;
                string value = "";
                byte[] data = new byte[nLen];
                for (int i = 0; i < nLen; i++)
                {
                    data[i] = byte.Parse(recv.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
                }
                //标签是否在线
                if ((data[0] & 0x40) != 0)
                {
                    return;
                }

                //当前标签显示的值
                for (int i = 0; i < 6; i++)
                {
                    value = value + Convert.ToChar(data[5 - i]);
                }

                //按键信息 
                if ((data[0] & 0x01) == 1)//key1
                {
                    keyNo = 1;
                }
                if ((data[0] & 0x02) == 2)//key2
                {
                    keyNo = 2;
                }
                if ((data[0] & 0x04) == 4)//key3
                {
                    keyNo = 3;
                }
                Ack(address, keyNo.ToString(), "key");
            }
        }

        public new void Start()
        {
            this.TimeOutEvent += new TimeOutEventHandler(ELabelOperator_TimeOutEvent);
            base.Start();
        }

        void ELabelOperator_TimeOutEvent()
        {
            if (Ack != null)
            {
                Ack(0, "数据发送超时！", "err");
            }
        }

        #region IELabelOperator 成员

        public event AckHandler Ack;

        public void SendData(int address, string[] data)
        {
            this.SendData(address, data[0]);
        }

        public bool IsComplete()
        {
            return DataQueue.Count == 0 ? true : false;
        }

        public string GetShowModeName()
        {
            return base.ShowModeName;
        }

        public void GetKey(int Address)
        {
            throw new NotImplementedException();
        }

        public void ClearDataQueue()
        {
            DataQueue.Clear();
        }

        #endregion

        #region IELabelOperator 成员


        public void SetFunction(byte useBkLight, byte lightFrequency, byte useInstructions, byte instructionsFrequency, byte useSinging, byte singFrequency)
        {
            throw new NotImplementedException();
        }

        public void SetFunctionType(byte type)
        {
            throw new NotImplementedException();
        }

        public void SetRowTextSize(byte firstRow, byte secondRow, byte thirdRow, byte fourthRow, byte fifthRow)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
