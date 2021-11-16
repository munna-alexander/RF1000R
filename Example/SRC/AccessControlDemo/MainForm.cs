using System;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace RFID_Sample
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            pollForCardTimer.Tick += new EventHandler(pollForCardTimerCallback);

            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = assembly.GetName();
            this.lb_Version.Text = "Version: V " + assemblyName.Version.ToString();
        }

        #region Constants and Definitions

        /// <summary>
        /// polling interval in milliseconds
        /// </summary>
        private const int POLL_INTERVAL = 100;

        /// <summary>
        /// Limit the log output.
        /// Length in characters. 
        /// </summary>
        private const int LOGBUFFERLENGTH = 40000;

        #endregion

        /// <summary>
        /// Start scanning of RFID reader with USB port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Members

        /// <summary>
        /// Reader class
        /// </summary>
        ReaderDLL reader = new ReaderDLL();

        /// <summary>
        /// Handle of reader
        /// </summary>
        Int32 ReaderHandle = -1;

        /// <summary>
        /// Timer thread for the scanning for cards 
        /// </summary>
        System.Windows.Forms.Timer pollForCardTimer = new System.Windows.Forms.Timer();

        /// <summary>
        /// Flag indicating that a card is currently detected 
        /// </summary>
        bool isCardDetected = false;

        /// <summary>
        /// LED color when a card is currently detected
        /// </summary>
        UInt16 ledColor = ReaderDLL.LED_OFF;

        /// <summary>
        /// Flag indicating dircection of outputstring of serial number
        /// </summary>
        bool isReverseUID = false;

        /// <summary>
        /// Card Types for setup function
        /// </summary>
        const byte CARD_TYPE_MIFARE = 0x10;
        const byte CARD_TYPE_LEGIC = 0x20;
        const byte CARD_TYPE_ISO15693 = 0x30;


        /// <summary>
        /// Limit the reader bufsize (default 128 byte).
        /// </summary>
        const int READER_BUFSIZE_HEADER = 6;                                                // param data include in buffer size  
        const int READERBUFSIZE_RF1070R = 1024;                                             // max bufsize include all bytes in the param data in the request or response telegram
        const int READERBUFSIZE_RF1060R = 512;                                              // max bufsize include all bytes in the param data in the request or response telegram        
        const int READERBUFSIZE_CONFIG = READERBUFSIZE_RF1060R;                             // max buffer size for both reader types
        const int READERBUFSIZE_USER = READERBUFSIZE_RF1060R - READER_BUFSIZE_HEADER;       // max user data buffer size for both reader types
        private const int V = 0x01;

        /// <summary>
        /// variable Card Type from vhl_select() call
        /// </summary>
        byte CardType = 0;

        /// <summary>
        /// Last Status
        /// </summary>

        enum baud_rate
        {
            ser_baud_300,
            ser_baud_600,
            ser_baud_1200,
            ser_baud_2400,
            ser_baud_4800,
            ser_baud_9600,
            ser_baud_14400,
            ser_baud_19200,
            ser_baud_28800,
            ser_baud_38400,
            ser_baud_57600,
            ser_baud_115200,
            ser_baud_576000,
            ser_baud_921600,
            ser_baud_500000
        };

        enum ser_parity
        {
            ser_par_none,
            ser_par_odd,
            ser_par_even
        };

        #endregion
        private void button_StartScan_USB(object sender, EventArgs e)
        {
            Int32 return_val = 0;
            Int32 Status = 0;

            if (ReaderHandle != -1)
            {
                // handle unexpected ReaderHandle
                if (ReaderHandle != 0)
                {
                    return_val = reader.brp_close_session(ReaderHandle);
                    if (bCheckForError(return_val, "brp_close_session"))
                    {
                        return;
                    }
                    logText("ERROR: Reader was allready opened!");
                    ReaderHandle = -1;
                }
                else
                {
                    logText("ERROR: Reader is allready opened!");
                }
                return;
            }

            if (radioButtonRS232.Checked == true)
            {
                // Read COM port from combobox

                int comport = comboBox_COM.SelectedIndex;
                
                if(comport == -1 )
                {
                    logText("ERROR : Please select a COM port !!!");
                    return;
                }

                // - open SERIAL port ---------------------------------------------------------------

                return_val = reader.brp_open_serial_session(ref ReaderHandle, comport /* COMX*/, (int)baud_rate.ser_baud_115200, (int)ser_parity.ser_par_none);
                if (return_val != 0)
                    ReaderHandle = -1;

                if (ReaderHandle == -1)
                {
                    logText(string.Format("SERIAL Card reader not found on COM Port: {0} ", comport + 1));
                    logText("Please check the COM interface : 115200 baud, parity none");
                    return;
                }

                // - set  checksum algorithm (default for usb is NONE) ------------------------------------
                Int32 checksum = ReaderDLL.BRP_CHECKSUM_BCC8;
                return_val = reader.brp_set_checksum(ReaderHandle, checksum);

                if (bCheckForError(return_val, "brp_set_checksum"))
                    logText("ERROR: set checksum algorithm");
                else
                    logText("Set checksum algorithm: BRP_CHECKSUM_BCC8");

            }
            else if(radioButtonUSB.Checked == true)
            {
                // - open USB port ---------------------------------------------------------------
                UInt32 productID = 0;
                return_val = reader.brp_open_usb_session(ref ReaderHandle, productID);
                if (bCheckForError(return_val, "brp_open_usb_session"))
                {
                    ReaderHandle = -1;
                    return;
                }
                logText("USB Card reader found");
            }
                  
            
            // - open set bufsize to READERBUFSIZE Byte -------------------------------------------------
            return_val = reader.brp_set_bufsize(ReaderHandle, READERBUFSIZE_CONFIG, READERBUFSIZE_CONFIG, READERBUFSIZE_CONFIG);
            if (bCheckForError(return_val, "brp_set_bufsize"))
            {
                ReaderHandle = -1;
                return;
            }
            logText(string.Format("max user bufsize {0:D} byte", READERBUFSIZE_USER));

            // - check device status ------------------------------------------------------------
            UInt32 boot_status = 0;
            return_val = reader.syscmd_get_boot_status(ReaderHandle, ref boot_status, ref Status);

            if (bCheckForError(return_val, "syscmd_get_boot_status"))
            {
                return_val = reader.syscmd_reset(ReaderHandle, ref Status);
                return_val = reader.brp_close_session(ReaderHandle);
                logText(string.Format("ERROR: Reader is not responding! syscmd_reset() was called"));

                ReaderHandle = -1;
                return;
            }

            logText(string.Format("BootStatus: " + boot_status.ToString() + " Status: " + Status.ToString()));

            //- get FW Version ----------------------------------------------------------------
            byte[] SnrByteArray = null;
            return_val = reader.syscmd_get_info(ReaderHandle, ref SnrByteArray, ref Status);
            if (bCheckForError(return_val, "syscmd_get_info"))
            {
                ReaderHandle = -1;
                return;
            }
            logText(string.Format("Firmware version: " + System.Text.Encoding.Default.GetString(SnrByteArray, 0 ,40)));

            if (radioB_Read_UID.Checked)
            {
                // - Scan for cards ---------------------------------------------------------------
                logText("");
                logText(string.Format("--------------- Reader is scanning for cards---"));
            }

            RWAdresse.ReadOnly = true; RWAdresse.BackColor = System.Drawing.SystemColors.Menu;
            RWLänge.ReadOnly = true; RWLänge.BackColor = System.Drawing.SystemColors.Menu;
            VHL_nummer.ReadOnly = true; VHL_nummer.BackColor = System.Drawing.SystemColors.Menu;
            RWData.ReadOnly = false; RWData.BackColor = System.Drawing.SystemColors.Menu;
            cb_ReverseUID.Enabled = true;
            WriteButton.Enabled = false;
            ReadButton.Enabled = false;
            radioB_Read_UID.Checked = true;
            bt_Clear.Enabled = true;
            bt_LedColor.Enabled = false;

            radioB_Read_UID.Enabled = true;

            radioB_autoread.Enabled = true;
            radioB_RW_manual.Enabled = true;
            radioB_Configcard.Enabled = true;
            radioB_ResetFactory.Enabled = true;

            radioButtonRS232.Enabled = false;
            radioButtonUSB.Enabled = false;
            isCardDetected = false;
            pollForCardTimer.Interval = POLL_INTERVAL;
            pollForCardTimer.Start();
        }

        /// <summary>
        /// Stops RFID reader
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_StopScan_Click(object sender, EventArgs e)
        {
            if (ReaderHandle != -1)
            {
                Int32 Status = 0;
                // - reset LED -------------------------------------------------------------------------
                reader.syscmd_set_port(ReaderHandle, ReaderDLL.LED_OFF, ref Status);

                reader.brp_close_session(ReaderHandle);
                logText("--------------- Reader stopped ---------------");
                logText("");
                ReaderHandle = -1;
            }
            pollForCardTimer.Stop();

            RWData.Clear();

            bt_LedColor.Enabled = false;
            VHL_nummer.ReadOnly = true; VHL_nummer.BackColor = System.Drawing.SystemColors.Menu;
            RWAdresse.ReadOnly = true; RWAdresse.BackColor = System.Drawing.SystemColors.Menu;
            RWLänge.ReadOnly = true; RWLänge.BackColor = System.Drawing.SystemColors.Menu;
            RWData.ReadOnly = true; RWData.BackColor = System.Drawing.SystemColors.Menu;
            ReadButton.Enabled = false;
            WriteButton.Enabled = false;
            radioB_Read_UID.Enabled = false;
            radioB_autoread.Enabled = false;
            radioB_RW_manual.Enabled = false;
            radioB_Configcard.Enabled = false;
            radioB_ResetFactory.Enabled = false;

            cb_ReverseUID.Enabled = false;

            radioButtonRS232.Enabled = true;
            radioButtonUSB.Enabled = true;
            bt_StartScan.Enabled = true;
        }

        /// <summary>
        /// Test LED
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_ChangeColLed_Click(object sender, EventArgs e)
        {
            switch (ledColor)
            {
                case ReaderDLL.LED_OFF:
                    ledColor = ReaderDLL.LED_GREEN;
                    logText(string.Format("Set LED green"));
                    break;

                case ReaderDLL.LED_GREEN:
                    ledColor = ReaderDLL.LED_RED;
                    logText(string.Format("Set LED red"));
                    break;

                case ReaderDLL.LED_RED:
                    ledColor = ReaderDLL.LED_ORANGE;
                    logText(string.Format("Set LED orange"));
                    break;

                case ReaderDLL.LED_ORANGE:
                    ledColor = ReaderDLL.BUZZER;
                    logText(string.Format("Set BUZZER ON"));
                    break;

                case ReaderDLL.BUZZER:
                    ledColor = ReaderDLL.LED_OFF;
                    logText(string.Format("Set BUZZER/LED off"));
                    break;
                                        
                default:
                    ledColor = ReaderDLL.LED_GREEN;
                    break;
            }

            // Set LED
            int Status = 0;
            int return_val = reader.syscmd_set_port(ReaderHandle, ledColor, ref Status);
            // check for error
            if (bCheckForError(return_val, "syscmd_set_port"))
            {
                return;
            }
        }

        /// <summary>
        /// Clear output field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_Clear_Click(object sender, EventArgs e)
        {
            tb_LogOutput.Clear();
            RWData.Clear();
            if(radioB_RW_manual.Checked)
                RWData.BackColor = System.Drawing.Color.White;
        }

        /// <summary>
        /// reverse the output string of the UID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_ReverseUID_CheckedChanged(object sender, EventArgs e)
        {
            isReverseUID = cb_ReverseUID.Checked;
        }
                  

        /// <summary>
        /// Timer callback function.
        /// Scans for cards in field
        /// </summary>
        /// <param name="myObject"></param>
        /// <param name="myEventArgs"></param>
        private unsafe void pollForCardTimerCallback(Object myObject, EventArgs myEventArgs)
        {
            int return_val = 0;

            if (radioB_autoread.Checked)
            {  // Is autoread is enabled - do not use VHL commands

                int Status = 0xFFFF;
                byte[] Data = new byte[9999];

                fixed (byte* Buf = Data)
                {
                    int resplen = 0;

                    if (isCardDetected == false)
                    {
                        // autoread data 
                        return_val = reader.brp_exec_command(ReaderHandle, 0x05, 0x01, Buf, 0, 100, ref Status, Buf, ref resplen, 9999);
                        if (return_val != ReaderDLL.BRP_OK || Status != ReaderDLL.BRP_OK)
                        {
                            if(Status != 0x501)   // NO_MESSAGE_ERR if card selected but not changed
                                logText(string.Format("ERROR: brp_exec_command(GetMessage) Status: 0x{0:X}, retval: 0x{0:X}", Status, return_val));
                        }
                        else
                        {
                            RWData.Clear();

                            logText(string.Format("-------------- GetMessage(..) OK  with Length:" + Data[1] + " -------- "));
                            for (int i = 2; i < Data[1] + 2; i++)        // offset is 2 in response frame;  Byte 1 is the lenght of telegarm
                            {   // start data after byte 2
                               RWData.Text += string.Format("{0:X2}", Data[i]);
                                RWData.Text += " ";
                            } 

                            *Buf = 0x00; // disable autoread before re-enabling it
                            return_val = reader.brp_exec_command(ReaderHandle, 0x05, 0x00, Buf, 1, 500, ref Status, Buf, ref resplen, 10);
                            if (return_val != ReaderDLL.BRP_OK)
                                logText(string.Format("ERROR: AR.SetMode(0): retval: 0x{0:X}",return_val));

                            isCardDetected = true;
                        }
                    }
                    else //   if (isCardDetected == true)
                    { // this block is needed for detecting the presence of the card
                        // check if the same card detected, then set LED green
                        return_val = reader.vhl_is_selected(ReaderHandle, ref Status);
                        if (return_val == ReaderDLL.BRP_OK && Status == ReaderDLL.BRP_OK)
                        {
                            reader.syscmd_set_port(ReaderHandle, ReaderDLL.LED_GREEN, ref Status);
                        }
                        else
                        {
                            RWData.Clear();  // if card not present clear output window
                            reader.syscmd_set_port(ReaderHandle, ReaderDLL.LED_OFF, ref Status);
                            isCardDetected = false;

                            *Buf = 0x0; // required for RF1060: disable HF before Autoread is continued
                            return_val = reader.brp_exec_command(ReaderHandle, 0x00, 0x01, Buf, 1, 500, ref Status, Buf, ref resplen, 100);
                            if (return_val != ReaderDLL.BRP_OK)
                                logText(string.Format("ERROR: Sys.HfReset: retval: 0x{0:X}", return_val));

                            // switch on the autoread mode with one shot
                           *Buf = 0x02; // set mode one shot
                           return_val = reader.brp_exec_command(ReaderHandle, 0x05, 0x00, Buf, 1, 500, ref Status, Buf, ref resplen, 10);   // SetMode with  500ms timeout , 100ms to fast
                            if (return_val != ReaderDLL.BRP_OK)
                                logText(string.Format("ERROR: AR.SetMode(2): retval: 0x{0:X}", return_val));
                        }
                    }
                }
            }
            else if(radioB_Read_UID.Checked)
            {    //  cyclic call vhl_select() and vhl_get_snr()
                Int32 Status = 0;

                if (!isCardDetected)
                {
                    // check if card is in field
                    bool Reselect = true;                 // search one card
                    bool AllowConfig = false;            // here false while Configcard not used
                    UInt16 CardTypeMask = 0xFFFF;

                    return_val = reader.vhl_select(ReaderHandle, CardTypeMask, Reselect, AllowConfig, ref CardType, ref Status);

                    // If no card is in field we are finished
                    if (Status == ReaderDLL.VHL_ERR_NOTAG || Status == ReaderDLL.VHL_ERR_HF)
                    {
                      return;
                    }

                    // check for error
                    // has to be after checking Status. Otherwise we get always the error: statuscode != 0
                    if (bCheckForError(return_val, "vhl_select"))
                    {
                       return;
                    }

                    // - We got a card. 
                    logText(string.Format("Card detected. Card type: 0x{0:X}", CardType));

                    // - Now we get the serial number ------------------------------------------------------------
                    byte[] SnrByteArray = null;
                    byte Length = 0;

                    return_val = reader.vhl_get_snr(ReaderHandle, ref SnrByteArray, ref Length, ref Status);
                    if (bCheckForError(return_val, "vhl_get_snr") || Length <= 0)
                    {
                      return;
                    }

                    StringBuilder serialNumberText = new StringBuilder("Serial Number: 0x");
                    for (int i = 0; SnrByteArray != null && i < SnrByteArray.Length; i++)
                    {
                       if (isReverseUID)
                       {
                           serialNumberText.Append(string.Format("{0:X2}", SnrByteArray[i]));
                       }
                       else
                       {
                          serialNumberText.Append(string.Format("{0:X2}", SnrByteArray[SnrByteArray.Length - 1 - i]));
                       }
                    }
                    logText(serialNumberText.ToString());

                    // Set LED
                    return_val = reader.syscmd_set_port(ReaderHandle, ledColor, ref Status);
                    // check for error
                    if (bCheckForError(return_val, "syscmd_set_port"))
                    {
                        return;
                    }
                    isCardDetected = true;
   
                    // ------------------------------------------------------
                    // Here you could add your code to handle "card in field"
                    //-------------------------------------------------------

                }
                else
                {
                    // - check if card has left the reader field ----------------------------------------------
                    return_val = reader.vhl_is_selected(ReaderHandle, ref Status);

                    // if card is still in field, we are finished 
                    if (return_val == ReaderDLL.BRP_OK && Status == ReaderDLL.BRP_OK)
                    {
                        return;
                    }

                    // check for error
                    if (Status != ReaderDLL.VHL_ERR_CARD_NOT_SELECTED && bCheckForError(return_val, "vhl_is_selected"))
                    {
                        return;
                    }

                    logText("Card left field");

                    // - reset LED -------------------------------------------------------------------------
                    return_val = reader.syscmd_set_port(ReaderHandle, ReaderDLL.LED_OFF, ref Status);
                    if (bCheckForError(return_val, "syscmd_set_port"))
                    {
                        return;
                    }
                    // ------------------------------------------------------
                    // Here you could add your code to handle "card leave field"
                    //-------------------------------------------------------
                    isCardDetected = false;

                }
            
                // check for error
                // has to be after checking Status. Otherwise we get always the error: statuscode != 0
                bCheckForError(return_val, "vhl_select");
            }
        }

        /// <summary>
        /// Read function for user memory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private unsafe void bt_ReadButton_Click(object sender, EventArgs e)
        {
            int retval = 0xFFFF;
            int Status = 0xFFFF;
            byte[] Data = new byte[READERBUFSIZE_CONFIG];

            bool Reselect = true;
            bool AllowConfig = false;           // here false while Configcard not used
            UInt16 CardTypeMask = 0xFFFF;
            int return_val = 0;

            RWData.Clear();
            RWData.BackColor = System.Drawing.Color.White;
                                 
            return_val = reader.vhl_select(ReaderHandle, CardTypeMask, Reselect, AllowConfig, ref CardType, ref Status);

            // If no card is in field we are finished
            if (Status == ReaderDLL.VHL_ERR_NOTAG || Status == ReaderDLL.VHL_ERR_HF || Status == ReaderDLL.VHL_ERR_CONFCARD_READ)
            {
                logText(string.Format("no Card detected. "));
                return;
            }
            logText(string.Format("Card detected. Card type: 0x{0:X}", CardType));



            //  Read user memory
            byte VHLFilenumber = Convert.ToByte(VHL_nummer.Text);
            short Address = Convert.ToInt16(RWAdresse.Text);
            short Length = Convert.ToInt16(RWLänge.Text);
            fixed (byte* Buf = Data)
            {
                // Read function with ReaderHandle from vhl_select() call, VHLFilenumber from the config file in tbe reader
                retval = reader.vhl_read(ReaderHandle, VHLFilenumber, Address, Length, Buf, ref Status);
            }

            if (Status != ReaderDLL.BRP_OK || retval != ReaderDLL.BRP_OK)
                logText(string.Format("ERROR: vhl_read() Status: 0x{0:X}, retval: 0x{1:X}", Status, retval));
            else
            {
                for (int i = 0; i < Length; i++)
                {
                    RWData.Text += string.Format("{0:X2}", Data[i]);
                    // set space between hex data when read
                    RWData.Text += " ";
                }
                logText(string.Format("-------------- vhl_read(..) OK  with Length:" + Length + " -------- "));
            }
        }

        /// <summary>
        /// Write function for user memory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private unsafe void bt_WriteButton_Click(object sender, EventArgs e)
        {
            short length = 0;
            int retval = 0xFFFF;
            int Status = 0xFFFF;
            byte[] frame = new byte[READERBUFSIZE_CONFIG];
            byte VHLFilenumber = Convert.ToByte(VHL_nummer.Text);
            short Address = Convert.ToInt16(RWAdresse.Text);
            string s = RWData.Text;
            string[] tokenArray = s.Split(' ', ',', ';', '\t');

            bool Reselect = true;
            bool AllowConfig = false;
            UInt16 CardTypeMask = 0xFFFF;
            int return_val = 0;

            return_val = reader.vhl_select(ReaderHandle, CardTypeMask, Reselect, AllowConfig, ref CardType, ref Status);

            // If no card is in field we are finished
            if (Status == ReaderDLL.VHL_ERR_NOTAG || Status == ReaderDLL.VHL_ERR_HF || Status == ReaderDLL.VHL_ERR_CONFCARD_READ)
            {
                logText(string.Format("no Card detected. "));
                return;
            }
            logText(string.Format("Card detected. Card type: 0x{0:X}", CardType));

            foreach (string token in tokenArray)
            {
                if (length < 9999)
                {
                    if (token != "" && token != " ")
                    {
                        try
                        {
                            frame[length++] = Convert.ToByte(token, 16);
                        }
                        catch (Exception)
                        {
                            RWData.BackColor = System.Drawing.Color.Red;
                            return;
                        }
                    }
                }
            }

            fixed (byte* Buf = frame)
            {
                // Write function with ReaderHandle from vhl_select() call, VHLFilenumber from the config file in tbe reader
                retval = reader.vhl_write(ReaderHandle, VHLFilenumber, Address, length, Buf, ref Status);
            }

            RWLänge.Text = Convert.ToString(length);

            if (Status != ReaderDLL.BRP_OK || retval != ReaderDLL.BRP_OK)
                logText(string.Format("ERROR: vhl_write() Status: 0x{0:X}, retval: 0x{1:X}", Status, retval));
            else
                logText(string.Format("-------------- vhl_write(..) OK  with Length:" + length + " -------- "));
        }

        /// <summary>
        /// content for checkbox serial number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void rb_SerNr_CheckedChanged(object sender, EventArgs e)
        {
            RWData.Clear();
            //tb_LogOutput.Clear();

            // - Scan for cards ---------------------------------------------------------------
            logText("");
            logText("--------------- Reader is scanning for cards---");

            RWAdresse.ReadOnly = true; RWAdresse.BackColor = System.Drawing.SystemColors.Menu;
            RWLänge.ReadOnly = true; RWLänge.BackColor = System.Drawing.SystemColors.Menu;
            VHL_nummer.ReadOnly = true; VHL_nummer.BackColor = System.Drawing.SystemColors.Menu;
            RWData.ReadOnly = true; RWData.BackColor = System.Drawing.SystemColors.Menu;

            cb_ReverseUID.Enabled = true;
            WriteButton.Enabled = false;
            ReadButton.Enabled = false;
            bt_LedColor.Enabled = false;

            isCardDetected = false;    // Select card in poll method
        }

        /// <summary>
        /// content for checkbox read auto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private unsafe void rb_autoread_CheckedChanged(object sender, EventArgs e)
        {
            // Menu controls
            RWData.Clear();
            //tb_LogOutput.Clear();

            RWAdresse.ReadOnly = true; RWAdresse.BackColor = System.Drawing.SystemColors.Menu;
            RWLänge.ReadOnly = true; RWLänge.BackColor = System.Drawing.SystemColors.Menu;
            VHL_nummer.ReadOnly = true; VHL_nummer.BackColor = System.Drawing.SystemColors.Menu;
            RWData.ReadOnly = true; RWData.BackColor = System.Drawing.SystemColors.Menu;

            cb_ReverseUID.Enabled = false;
            WriteButton.Enabled = false;
            ReadButton.Enabled = false;
            bt_LedColor.Enabled = true;
            isCardDetected = false;

            int retval = 0xFFFF;
            int Status = 0xFFFF;
            byte[] Data = new byte[10];

            if (radioB_autoread.Checked)
            {
                Data[0] = 0x02;  // write SetMode (2) for autoread one shot
                logText(string.Format("A autoread configuration is required !!"));
            }
            else
                Data[0] = 0x00;  // write SetMode (0) for disable autoread 

            fixed (byte* Buf = Data)
            {
                int resplen = 0;
                retval = reader.brp_exec_command(ReaderHandle, 0x05, 0x00, Buf, 1, 500, ref Status, Buf, ref resplen, 10);   // SetMode with  500ms timeout , 100ms to fast

                if (retval != ReaderDLL.BRP_OK || Status != ReaderDLL.BRP_OK)
                    logText(string.Format("ERROR: Autoread SetMode Status: 0x{0:X}, retval: 0x{1:X}", Status, retval));
                else
                    logText(string.Format("Autoread SetMode with param = 0x{0:X} OK", Data[0]));
            }
        }

        /// <summary>
        /// content for checkbox read/write manually
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rb_rw_manualCheckedChanged(object sender, EventArgs e)
        {
            // Menu controls
            RWData.Clear();
            //tb_LogOutput.Clear();

            RWAdresse.ReadOnly = false; RWAdresse.BackColor = System.Drawing.Color.White;
            RWLänge.ReadOnly = false; RWLänge.BackColor = System.Drawing.Color.White;
            VHL_nummer.ReadOnly = false; VHL_nummer.BackColor = System.Drawing.Color.White;
            RWData.ReadOnly = false; RWData.BackColor = System.Drawing.Color.White;

            cb_ReverseUID.Enabled = false;
            WriteButton.Enabled = true;
            ReadButton.Enabled = true;
            bt_LedColor.Enabled = true;

        }

        private unsafe void radioB_Configcard_CheckedChanged(object sender, EventArgs e)
        {
            // Menu controls
            RWData.Clear();
            int Status = 0xFFFF;
            byte[] Data = new byte[10];
            
            if (radioB_Configcard.Checked)
            {
                // - Mode Configcard read 
                logText(string.Format("Start Configcard (Auto)Read Mode."));
                Data[0] = 0x01;  // write SetMode (2) for autoread 
            }
            else
            {
                return;
            }

            fixed (byte* Buf = Data)
            {
                int resplen = 0;
                int retval = reader.brp_exec_command(ReaderHandle, 0x05, 0x00, Buf, 1, 500, ref Status, Buf, ref resplen, 10);   // SetMode with  500ms timeout , 100ms to fast

                if (retval != ReaderDLL.BRP_OK || Status != ReaderDLL.BRP_OK)
                    logText(string.Format("ERROR: Autoread SetMode Status: 0x{0:X}, retval: 0x{1:X}", Status, retval));
            }

                logText(string.Format("Please hold Configcard to the reader."));
                logText(string.Format("Look at the lamp image. Red LED means error !"));
                logText(string.Format("Wait !"));

            RWAdresse.ReadOnly = true; RWAdresse.BackColor = System.Drawing.SystemColors.Menu;
            RWLänge.ReadOnly = true; RWLänge.BackColor = System.Drawing.SystemColors.Menu;
            VHL_nummer.ReadOnly = true; VHL_nummer.BackColor = System.Drawing.SystemColors.Menu;
            RWData.ReadOnly = true; RWData.BackColor = System.Drawing.SystemColors.Menu;

            cb_ReverseUID.Enabled = false;
            WriteButton.Enabled = false;
            ReadButton.Enabled = false;
            bt_LedColor.Enabled = false;
            radioB_Read_UID.Enabled = false;
            radioB_autoread.Enabled = false;
            radioB_RW_manual.Enabled = false;
            radioB_Configcard.Enabled = false;

            bt_StopScan.Enabled = false;
            bt_StartScan.Enabled = false;
            bt_Clear.Enabled = false;

            System.Threading.Thread.Sleep(10000);             // wait for reboot reader
          
            logText(string.Format("Press Button STOP and START again !"));

            bt_StopScan.Enabled = true;
            bt_StartScan.Enabled = false;
        }

#region Helper functions

        /// <summary>
        /// Helper function. Check for malfunctions of reader
        /// </summary>
        /// <param name="return_val"></param>
        /// <param name="functionName"></param>
        /// <returns></returns>
        private bool bCheckForError(Int32 return_val, string functionName)
        {
            if (return_val != ReaderDLL.BRP_OK)
            {
                logText("ERROR in " + functionName + "!\r\n\t\t\tReturn value=" + return_val.ToString() + " :" + reader.getStringFromErrorCode(return_val));
                //bt_StopScan_Click(this, EventArgs.Empty);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Print text into output box
        /// </summary>
        /// <param name="text"></param>
        private void logText(string text)
        {
            tb_LogOutput.AppendText(createTimeStampString() + "\t" + text + "\r\n");
            
            // Keep the contents of the informationbox at a reasonable size
            if (tb_LogOutput.TextLength > LOGBUFFERLENGTH)
            {
                tb_LogOutput.Clear();
            }
        }

        /// <summary>
        /// Create time stamp string
        /// </summary>
        /// <returns></returns>
        private string createTimeStampString()
        {
            string timeStampString = String.Empty;
            DateTime time = DateTime.Now;

            // use language independent format for logging. 
            string dateString = String.Empty; //  time.Day.ToString("00") + DATE_SEPARATOR + time.Month.ToString("00") + DATE_SEPARATOR + time.Year.ToString();
            string timeString = time.Hour.ToString("00") + ":" + time.Minute.ToString("00") + ":" + time.Second.ToString("00") + "." + time.Millisecond.ToString("000");
            timeStampString = dateString + " " + timeString;
            return timeStampString;
        }
#endregion

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void VHL_nummer_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(VHL_nummer.Text) > 255)
                    VHL_nummer.Text = "0";
                VHL_nummer.BackColor = System.Drawing.Color.White;
            }
            catch (Exception)
            {
                VHL_nummer.BackColor = System.Drawing.Color.Red;
                VHL_nummer.Text = "0";
            }
        }

        private void RWAdresse_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Convert.ToInt32(RWAdresse.Text);
                RWAdresse.BackColor = System.Drawing.Color.White;
            }
            catch (Exception)
            {
                RWAdresse.BackColor = System.Drawing.Color.Red;
                RWAdresse.Text = "0";
            }
        }

        private void RWLänge_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if(Convert.ToInt32(RWLänge.Text) > READERBUFSIZE_USER)
                    RWLänge.Text = "0";
                RWLänge.BackColor = System.Drawing.Color.White;
            }
            catch (Exception)
            {
                RWLänge.BackColor = System.Drawing.Color.Red;
                RWLänge.Text = "0";
            }
        }

        private void RWData_TextChanged(object sender, EventArgs e)
        {
        }

        private void Date_Label_Click(object sender, EventArgs e)
        {

        }

        private void RWData_KeyPress(object sender, KeyPressEventArgs e)
        {
            // check if hexadecimal input
            char c = e.KeyChar;

            if (!((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f') || c == '\b' || c == ' ') )
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
                if(radioB_RW_manual.Checked)
                    RWData.BackColor = System.Drawing.Color.White;
                return;
            }
        }

        private void tb_LogOutput_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButtonRS232_CheckedChanged(object sender, EventArgs e)
        {
            comboBox_COM.Enabled = true;
        }

        private void radioButtonUSB_CheckedChanged(object sender, EventArgs e)
        {
            comboBox_COM.Enabled = false;
        }

        private unsafe void radioB_ResetFactory_CheckedChanged(object sender, EventArgs e)
        {
            int Status = 0x00;
            byte[] Data = new byte[2];

            if (radioB_ResetFactory.Checked)
            {
                int return_val = 0;
                fixed (byte* param = Data)
                {
                    *param = 0x01; // // PerformReboot
                    int resplen = 0;

                    return_val = reader.brp_exec_command(ReaderHandle, 0x00, 0x28, param, 1, 15000, ref Status, param, ref resplen, 0);
                    if (return_val != ReaderDLL.BRP_OK || Status != ReaderDLL.BRP_OK)
                    {
                        logText(string.Format("ERROR: brp_exec_command(ResetFactory) Status: 0x{0:X}, retval: 0x{0:X}", Status, return_val));
                        logText(string.Format("Press Button STOP and START again !"));
                        return;
                    }
                }

                RWAdresse.ReadOnly = true; RWAdresse.BackColor = System.Drawing.SystemColors.Menu;
                RWLänge.ReadOnly = true; RWLänge.BackColor = System.Drawing.SystemColors.Menu;
                VHL_nummer.ReadOnly = true; VHL_nummer.BackColor = System.Drawing.SystemColors.Menu;
                RWData.ReadOnly = true; RWData.BackColor = System.Drawing.SystemColors.Menu;

                cb_ReverseUID.Enabled = false;
                WriteButton.Enabled = false;
                ReadButton.Enabled = false;
                bt_LedColor.Enabled = false;
                radioB_Read_UID.Enabled = false;
                radioB_autoread.Enabled = false;
                radioB_RW_manual.Enabled = false;
                radioB_Configcard.Enabled = false;

                bt_StopScan.Enabled = false;
                bt_StartScan.Enabled = false;
                bt_Clear.Enabled = false;
                radioB_ResetFactory.Enabled = false;

                logText(string.Format("Pleaes wait for reboot 5 seconds"));

                System.Threading.Thread.Sleep(5000);             // wait for reboot reader

                if (return_val == ReaderDLL.BRP_OK && Status == ReaderDLL.BRP_OK)
                {
                    logText(string.Format("Reset to Factory completed !"));
                    logText(string.Format("Press Button STOP and START again !"));
                }

                bt_StopScan.Enabled = true;
                bt_StartScan.Enabled = false;
            }
        }
    }
}
