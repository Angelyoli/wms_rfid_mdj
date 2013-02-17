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
        /// ����������ɫ
        /// </summary>
        /// <param name="color"></param>
        public void SetShowColor(TextColor color)
        {
            ((Protocal)this.Com.Encoder).BtShowColor = (byte)color;
        }
        /// <summary>
        /// ���ñ�ǩ��ʾ����
        /// </summary>
        /// <param name="showModel">ShowDeviceָʾ�豸,TwoBitShowDevice��λ����</param>
        public void SetFunctionType(ShowModel showModel)
        {
            ((Protocal)this.Com.Encoder).FormatType = (byte)showModel;
        }
        /// <summary>
        /// ������˸״̬
        /// </summary>
        /// <param name="flashState">����,������˸,������˸</param>
        /// <param name="flashModel">CONFIRMϨ�𷵻ذ���,���ݼ�����00000����CONFIRMϨ��,��CONFIRM����˸5�κ�Ϩ�𷵻ص�ǰ��ֵ</param>
        /// <param name="flashSwitchBit"></param>
        public void SetFlashState(FlashState flashState, FlashModel flashModel, int flashSwitchBit)
        {
            byte[] switchBit = BitConverter.GetBytes(flashSwitchBit);
            ((Protocal)this.Com.Encoder).BtFlashState[0] = switchBit[0];
            ((Protocal)this.Com.Encoder).BtFlashState[1] = (byte)((switchBit[1] & 0X3FFF) ^ (byte)((byte)flashState << 6));
            ((Protocal)this.Com.Encoder).BtFlashState[2] = (byte)flashModel;
        }
        /// <summary>
        /// ���ñ����ƹ⣬ָʾ�ƣ���������
        /// </summary>
        /// <param name="useBkLight">�����ֶ�</param>
        /// <param name="lightFrequency">�����ֶ�Ƶ��</param>
        /// <param name="useInstructions">ָʾ��</param>
        /// <param name="instructionsFrequency">ָʾ����˸Ƶ��</param>
        /// <param name="useSinging">������</param>
        /// <param name="singFrequency">����������Ƶ��</param>
        public void SetFunction(FuntionState useBkLight, byte lightFrequency, FuntionState useInstructions,
            byte instructionsFrequency, FuntionState useSinging, byte singFrequency)
        {
            ((Protocal)this.Com.Encoder).BtFunction[0] = (byte)((byte)useBkLight * 64 | lightFrequency);
            ((Protocal)this.Com.Encoder).BtFunction[1] = (byte)((byte)useInstructions * 64 | instructionsFrequency);
            ((Protocal)this.Com.Encoder).BtFunction[2] = (byte)((byte)useSinging * 64 | singFrequency);
        }

        /// <summary>
        /// ����״̬��Ƶ������
        /// </summary>
        /// <param name="state1">����1״̬</param>
        /// <param name="frequency1">����1��˸Ƶ��</param>
        /// <param name="state2">����2״̬</param>
        /// <param name="frequency2">����2��˸Ƶ��</param>
        /// <param name="state3">����3״̬</param>
        /// <param name="frequency3">����3��˸Ƶ��</param>
        public void SetKeysState(FuntionState state1, int frequency1, FuntionState state2, int frequency2, FuntionState state3, int frequency3)
        {
            ((Protocal)this.Com.Encoder).BtKeyLightState[0] = (byte)((byte)state1 * 64 | frequency1);
            ((Protocal)this.Com.Encoder).BtKeyLightState[1] = (byte)((byte)state2 * 64 | frequency2);
            ((Protocal)this.Com.Encoder).BtKeyLightState[2] = (byte)((byte)state3 * 64 | frequency3);
        }

        /// <summary>
        /// �������ӱ�ǩ
        /// </summary>
        /// <param name="address">���ӱ�ǩ��ַ</param>
        /// <param name="cmdType">��������</param>
        private void ExecuteCmd(int address, CmdType cmdType ,object data)
        {
            CmdData cmddata = new CmdData();
            cmddata.address = address;
            cmddata.cmdType = cmdType;
            cmddata.data = data;
            this.Send(cmddata);
        }

        /// <summary>
        /// �������ݵ����ӱ�ǩ,��ǩʱʱ���
        /// </summary>
        /// <param name="address">���ӱ�ǩ��ַ</param>
        /// <param name="data">����</param>
        public void SendData(int address, string data)
        {
            ExecuteCmd(address, CmdType.SendData, data);
        }
        
        /// <summary>
        /// �������ݵ����ӱ�ǩ
        /// </summary>
        /// <param name="address">���ӱ�ǩ��ַ</param>
        /// <param name="data">����</param>
        public void ShowData(int address, string data)
        {
            ExecuteCmd(address, CmdType.ShowOnly, data);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="address">���ӱ�ǩ��ַ</param>
        public void LoudSpeaker(int address)
        {
            byte[] btData = new byte[2];
            btData[1] = 1;
            ExecuteCmd(address, CmdType.LoudSpeaker, btData);
        }
        /// <summary>
        /// �ص�
        /// </summary>
        /// <param name="address">���ӱ�ǩ��ַ</param>
        public void CloseLight(int address)
        {
            byte[] btData = new byte[2];
            ExecuteCmd(address, CmdType.CloseLight, btData);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="address">���ӱ�ǩ��ַ</param>
        public void OpenLight(int address)
        {
            byte[] btData = new byte[2];
            btData[1] = 1;
            ExecuteCmd(address, CmdType.OpenLight, btData);
        }
        /// <summary>
        /// ��λ
        /// </summary>
        /// <param name="address">���ӱ�ǩ��ַ</param>
        public void ResetElectronicLabel(int address)
        {
            byte[] btData = new byte[2];
            ExecuteCmd(address, CmdType.ResetElectronicLabel, btData);
        }
        /// <summary>
        /// ���ư�ID
        /// </summary>
        /// <param name="address">���ӱ�ǩ��ַ</param>
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
                    Ack(pr.address, "���ƿ���������ʶ�����", "err");
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
                    Ack(pr.address, "SRamFull��", "err");
                    break;
                case RecvType.DeleteTable:
                    //this.DeleteData(pr.address);
                    Ack(pr.address, "DeleteTable��", "err");
                    break;
                case RecvType.ResetCorrect:
                    Ack(pr.address, "���ƿ���λ�ɹ���", "success");
                    break;
                case RecvType.ResetError:
                    Ack(pr.address, "���ƿ���λʧ�ܣ�", "err");
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
                //��ǩ�Ƿ�����
                if ((data[0] & 0x40) != 0)
                {
                    return;
                }

                //��ǰ��ǩ��ʾ��ֵ
                for (int i = 0; i < 6; i++)
                {
                    value = value + Convert.ToChar(data[5 - i]);
                }

                //������Ϣ 
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
                Ack(0, "���ݷ��ͳ�ʱ��", "err");
            }
        }

        #region IELabelOperator ��Ա

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

        #region IELabelOperator ��Ա


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
