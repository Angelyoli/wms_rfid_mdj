using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace THOK.Zeng.ComfixtureHandle.el102
{
    public class ELabelOperator : THOK.Zeng.ComfixtureHandle.ComfixtureHandle, THOK.Zeng.ComfixtureHandle.IELabelOperator 
    {
        public event AckHandler Ack;

        public void SetFunction(byte useBkLight, byte lightFrequency, byte useInstructions, byte instructionsFrequency, byte useSinging, byte singFrequency)
        {
            ((Protocal)this.Com.Encoder).BtFunction[0] = (byte)((useBkLight * 0x40) | lightFrequency);
            ((Protocal)this.Com.Encoder).BtFunction[1] = (byte)((useInstructions * 0x40) | instructionsFrequency);
            ((Protocal)this.Com.Encoder).BtFunction[2] = (byte)((useSinging * 0x40) | singFrequency);
        }

        public void SetFunctionType(byte type)
        {
            ((Protocal)this.Com.Encoder).FormatType = type;
        }

        public void SetRowTextSize(byte firstRow, byte secondRow, byte thirdRow, byte fourthRow, byte fifthRow)
        {
            ((Protocal)this.Com.Encoder).BtRowTextSize[0] = firstRow;
            ((Protocal)this.Com.Encoder).BtRowTextSize[1] = secondRow;
            ((Protocal)this.Com.Encoder).BtRowTextSize[2] = thirdRow;
            ((Protocal)this.Com.Encoder).BtRowTextSize[3] = fourthRow;
            ((Protocal)this.Com.Encoder).BtRowTextSize[4] = fifthRow;
        }

        public override void com_Data_Return(object Data)
        {
            try
            {
                PacketRecv pr = (PacketRecv)Data;
                if (pr.recvType != RecvType.Correct && Ack == null)
                {
                    return;
                }
                switch (pr.recvType)
                {
                    case RecvType.Correct:
                        if (this.CurrentData != null && pr.address == ((CmdData)this.CurrentData.data).address)
                        {
                            this.CurrentData = null;
                        }
                        break;
                    case RecvType.KeyEvent:
                        if (this.CurrentData != null && pr.address == ((CmdData)this.CurrentData.data).address)
                        {
                            this.CurrentData = null;
                        }
                        string recv = pr.message;
                        string data = recv.Substring(6, 2);
                        byte[] btData = new byte[2];
                        int keyNo = 0;
                        btData[0] = byte.Parse(data.Substring(0, 2), NumberStyles.HexNumber);

                        if ((btData[0] & 1) == 1)
                        {
                            keyNo = 1;
                        }
                        if ((btData[0] & 2) == 2)
                        {
                            keyNo = 2;
                        }
                        if ((btData[0] & 4) == 4)
                        {
                            keyNo = 3;
                        }

                        Ack(pr.address % 256 , keyNo.ToString(), "key");
                        break;
                    case RecvType.DeleteTable:
                        this.DeleteData(pr.address);
                        Ack(pr.address, "DeleteTable！", "err");
                        break;
                    case RecvType.CheckError :
                        Ack(pr.address, "CheckError", "err");
                        break;
                    case RecvType.EndError:
                        Ack(pr.address, "EndError", "err");
                        break;
                    case RecvType.NotExist:
                        Ack(pr.address, "NotExist", "err");
                        break;
                    case RecvType.SRamFull:
                        Ack(pr.address, "SRamFull", "err");
                        break;
                    case RecvType.Timeout:
                        Ack(pr.address, "Timeout", "err");
                        break;
                    case RecvType.ResetCorrect:
                        Ack(pr.address, "控制卡复位成功！", "success");
                        break;
                    case RecvType.ResetError:
                        Ack(pr.address, "控制卡复位失败！", "err");
                        break;
                    case RecvType.Others:
                        Ack(pr.address, "控制卡其他不可识别错误！", "err");
                        break;
                    default:
                        break;
                }
            }
            catch { }
        }
        public void ResetCotrolPlate()
        {
            byte[] btData = new byte[2];
            this.ExecuteCmd(0, CmdType.ResetControlPlate, btData);
        }

        public void ResetElectronicLabel(int address)
        {
            byte[] btData = new byte[2];
            this.ExecuteCmd(address, CmdType.ResetElectronicLabel, btData);
        }
        public void ShowControlPlateID(int id)
        {
            byte[] btData = new byte[2];
            btData[1] = (byte)(id % 0x100);
            this.ExecuteCmd(0, CmdType.ControlPlateID, btData);
        }
        public void SendData(int address, string[] data)
        {
            string[] strData = new string[data.Length];
            data.CopyTo(strData, 0);
            this.ExecuteCmd(address, CmdType.SendData, strData);
        }
        public void GetKey(int address)
        {
            byte[] btData = new byte[2];
            this.ExecuteCmd(address, CmdType.GetKey , btData);
        }
        public void DeleteData(int address)
        {
            byte[] btData = new byte[2];
            btData[0] = (byte)(address % 0x100);
            this.ExecuteCmd(address, CmdType.Delete, btData);
        }

        private void ExecuteCmd(int address, CmdType cmdType, object data)
        {
            CmdData  cmddata = new CmdData();
            cmddata.address = address + 256 * (base.Com.port - 2);
            cmddata.cmdType = cmdType;
            cmddata.data = data;
            this.Send(cmddata);
        }
        public new void Start()
        {
            this.TimeOutEvent += new TimeOutEventHandler(ELabelOperator_TimeOutEvent);
            this.ResetCotrolPlate();
            base.Start();          
        }

        void ELabelOperator_TimeOutEvent()
        {
            if (Ack != null )
            {
                Ack(0, "数据发送超时！", "err");
            }
        }

        #region IELabelOperator 成员


        public bool IsComplete()
        {
            return DataQueue.Count == 0 ? true : false;
        }

        public string GetShowModeName()
        {
            return base.ShowModeName;
        }

        public void ClearDataQueue()
        {
            DataQueue.Clear();
        }

        #endregion

        #region IELabelOperator 成员


        public void SendData(int address, string data)
        {
            throw new NotImplementedException();
        }

        public void SetFlashState(THOK.Zeng.ComfixtureHandle.el103.FlashState flashState, THOK.Zeng.ComfixtureHandle.el103.FlashModel flashModel, int flashSwitchBit)
        {
            throw new NotImplementedException();
        }

        public void SetFunction(THOK.Zeng.ComfixtureHandle.el103.FuntionState useBkLight, byte lightFrequency, THOK.Zeng.ComfixtureHandle.el103.FuntionState useInstructions, byte instructionsFrequency, THOK.Zeng.ComfixtureHandle.el103.FuntionState useSinging, byte singFrequency)
        {
            throw new NotImplementedException();
        }

        public void SetFunctionType(THOK.Zeng.ComfixtureHandle.el103.ShowModel showModel)
        {
            throw new NotImplementedException();
        }

        public void SetKeysState(THOK.Zeng.ComfixtureHandle.el103.FuntionState state1, int frequency1, THOK.Zeng.ComfixtureHandle.el103.FuntionState state2, int frequency2, THOK.Zeng.ComfixtureHandle.el103.FuntionState state3, int frequency3)
        {
            throw new NotImplementedException();
        }

        public void SetShowColor(THOK.Zeng.ComfixtureHandle.el103.TextColor color)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
