using System;
using THOK.Zeng.ComfixtureHandle.el103;
namespace THOK.Zeng.ComfixtureHandle
{
    public interface IELabelOperator:IDisposable 
    {
        event AckHandler Ack;
        void ResetCotrolPlate();
        void ResetElectronicLabel(int address);
        void SendData(int address, string[] data);
        void SetFunction(byte useBkLight, byte lightFrequency, byte useInstructions, byte instructionsFrequency, byte useSinging, byte singFrequency);
        void SetFunctionType(byte type);
        void SetRowTextSize(byte firstRow, byte secondRow, byte thirdRow, byte fourthRow, byte fifthRow);
        void ShowControlPlateID(int id);
        void Start();

        bool IsComplete();
        string GetShowModeName();
        void GetKey(int Address);
        void ClearDataQueue();

        void SetFlashState(FlashState flashState, FlashModel flashModel, int flashSwitchBit);
        void SetFunction(FuntionState useBkLight, byte lightFrequency, FuntionState useInstructions, byte instructionsFrequency, FuntionState useSinging, byte singFrequency);
        void SetFunctionType(ShowModel showModel);
        void SetKeysState(FuntionState state1, int frequency1, FuntionState state2, int frequency2, FuntionState state3, int frequency3);
        void SetShowColor(TextColor color);
    }
    public delegate void AckHandler(int address, string msg, string type);
}
