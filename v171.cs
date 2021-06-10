using FlightPlotter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace v171
{
	public class AlogData
	{
		public List<string> log_autopilot { get; set; } = new List<string>();

		public void Clear()
		{
			log_autopilot.Clear();
		}

		public void ReadData(int length, Stream stream)
        {
			log_autopilot.Add(Reader.ReadLogValue(stream, length));
        }
	}

	public class SessData
	{
		public List<string> log_sess { get; set; } = new List<string>();

		public void Clear()
		{
			log_sess.Clear();
		}
		public void ReadData(int length, Stream stream)
		{
			log_sess.Add(Reader.ReadLogValue(stream, length));
		}
	}

	public class MetaData
	{
		public List<string> meta { get; set; } = new List<string>();

		public void Clear()
		{
			meta.Clear();
		}
		public void ReadData(int length, Stream stream)
		{
			meta.Add(Reader.ReadLogValue(stream, length));
		}
	}

	public class ClogData
	{
		public List<string> log_copilot { get; set; } = new List<string>();

		public void Clear()
		{
			log_copilot.Clear();
		}
		public void ReadData(int length, Stream stream)
		{
			log_copilot.Add(Reader.ReadLogValue(stream, length));
		}
	}

	public class SnsrData
	{
		public DateTime OldDT { get; set; } = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
		public List<double> meas_w_B_ECI_B1 { get; set; } = new List<double>();
		public List<double> meas_w_B_ECI_B2 { get; set; } = new List<double>();
		public List<double> meas_w_B_ECI_B3 { get; set; } = new List<double>();
		public List<double> filt_w_B_ECI_B1 { get; set; } = new List<double>();
		public List<double> filt_w_B_ECI_B2 { get; set; } = new List<double>();
		public List<double> filt_w_B_ECI_B3 { get; set; } = new List<double>();
		public List<double> q_B_NED1 { get; set; } = new List<double>();
		public List<double> q_B_NED2 { get; set; } = new List<double>();
		public List<double> q_B_NED3 { get; set; } = new List<double>();
		public List<double> q_B_NED4 { get; set; } = new List<double>();
		public List<double> Euler321_B_NED_hs_psi_hs { get; set; } = new List<double>();
		public List<double> Euler321_B_NED_hs_theta_hs { get; set; } = new List<double>();
		public List<double> Euler321_B_NED_hs_phi_hs { get; set; } = new List<double>();
		public List<double> Euler312_B_NED_hv_psi { get; set; } = new List<double>();
		public List<double> Euler312_B_NED_hv_phi { get; set; } = new List<double>();
		public List<double> Euler312_B_NED_hv_theta { get; set; } = new List<double>();
		public List<double> filt_airspeed { get; set; } = new List<double>();
		public List<sbyte> airspeed_invalid { get; set; } = new List<sbyte>();
		public List<double> altimeter { get; set; } = new List<double>();
		public List<double> est_alt { get; set; } = new List<double>();
		public List<double> est_hdot { get; set; } = new List<double>();
		public List<float> AGL_bias { get; set; } = new List<float>();
		public List<double> time_unix { get; set; } = new List<double>();
		public List<sbyte> pss8_alt_invalid { get; set; } = new List<sbyte>();
		public List<double> Vel_B_ECF_NED1 { get; set; } = new List<double>();
		public List<double> Vel_B_ECF_NED2 { get; set; } = new List<double>();
		public List<double> Vel_B_ECF_NED3 { get; set; } = new List<double>();
		public List<double> sog { get; set; } = new List<double>();
		public List<double> cog { get; set; } = new List<double>();
		public List<double> rc_input_rc_aileron { get; set; } = new List<double>();
		public List<double> rc_input_rc_elevator { get; set; } = new List<double>();
		public List<double> rc_input_rc_flap { get; set; } = new List<double>();
		public List<double> rc_input_rc_gear { get; set; } = new List<double>();
		public List<double> rc_input_rc_rudder { get; set; } = new List<double>();
		public List<double> rc_input_rc_throttle { get; set; } = new List<double>();
		public List<short> rc_input_rc_man_bckup_lsb { get; set; } = new List<short>();
		public List<sbyte> rc_control_en { get; set; } = new List<sbyte>();
		public List<double> pos_NE1 { get; set; } = new List<double>();
		public List<double> pos_NE2 { get; set; } = new List<double>();
		public List<double> alt_INS { get; set; } = new List<double>();
		public List<double> meas_lat { get; set; } = new List<double>();
		public List<double> meas_lng { get; set; } = new List<double>();
		public List<sbyte> gps_ready { get; set; } = new List<sbyte>();
		public List<double> init_lat_lon1 { get; set; } = new List<double>();
		public List<double> init_lat_lon2 { get; set; } = new List<double>();
		public List<sbyte> pos_initialized { get; set; } = new List<sbyte>();
		public List<float> Accel_NED1 { get; set; } = new List<float>();
		public List<float> Accel_NED2 { get; set; } = new List<float>();
		public List<float> Accel_NED3 { get; set; } = new List<float>();
		public List<float> est_pos_NE1 { get; set; } = new List<float>();
		public List<float> est_pos_NE2 { get; set; } = new List<float>();
		public List<float> est_vel_NE1 { get; set; } = new List<float>();
		public List<float> est_vel_NE2 { get; set; } = new List<float>();
		public List<float> Accel_B1 { get; set; } = new List<float>();
		public List<float> Accel_B2 { get; set; } = new List<float>();
		public List<float> Accel_B3 { get; set; } = new List<float>();
		public List<double> linux_time { get; set; } = new List<double>();
		public List<sbyte> INS_solution_valid { get; set; } = new List<sbyte>();
		public List<double> est_alt_bias { get; set; } = new List<double>();
		public List<byte> alt_limit_ctr { get; set; } = new List<byte>();
		public List<float> alt_meas_residual { get; set; } = new List<float>();
		public List<double> vectornav_tlm_rawPositionLat { get; set; } = new List<double>();
		public List<double> vectornav_tlm_rawPositionLon { get; set; } = new List<double>();
		public List<double> vectornav_tlm_rawPositionAlt { get; set; } = new List<double>();
		public List<float> vectornav_tlm_rawVel_N { get; set; } = new List<float>();
		public List<float> vectornav_tlm_rawVel_E { get; set; } = new List<float>();
		public List<float> vectornav_tlm_rawVel_D { get; set; } = new List<float>();
		public List<float> vectornav_tlm_uncmpAngRate_Bx { get; set; } = new List<float>();
		public List<float> vectornav_tlm_uncmpAngRate_By { get; set; } = new List<float>();
		public List<float> vectornav_tlm_uncmpAngRate_Bz { get; set; } = new List<float>();
		public List<float> vectornav_tlm_accel_Bx { get; set; } = new List<float>();
		public List<float> vectornav_tlm_accel_By { get; set; } = new List<float>();
		public List<float> vectornav_tlm_accel_Bz { get; set; } = new List<float>();
		public List<float> vectornav_tlm_uncmpAccel_Bx { get; set; } = new List<float>();
		public List<float> vectornav_tlm_uncmpAccel_By { get; set; } = new List<float>();
		public List<float> vectornav_tlm_uncmpAccel_Bz { get; set; } = new List<float>();
		public List<float> vectornav_tlm_mag_Bx { get; set; } = new List<float>();
		public List<float> vectornav_tlm_mag_By { get; set; } = new List<float>();
		public List<float> vectornav_tlm_mag_Bz { get; set; } = new List<float>();
		public List<ushort> vectornav_tlm_sensSat { get; set; } = new List<ushort>();
		public List<float> vectornav_tlm_yawU { get; set; } = new List<float>();
		public List<float> vectornav_tlm_pitchU { get; set; } = new List<float>();
		public List<float> vectornav_tlm_rollU { get; set; } = new List<float>();
		public List<float> vectornav_tlm_posU { get; set; } = new List<float>();
		public List<float> vectornav_tlm_velU { get; set; } = new List<float>();
		public List<float> vectornav_tlm_GPS1GDOP { get; set; } = new List<float>();
		public List<float> vectornav_tlm_GPS1TDOP { get; set; } = new List<float>();
		public List<float> vectornav_tlm_GPS2GDOP { get; set; } = new List<float>();
		public List<byte> vectornav_tlm_numGPS1Sats { get; set; } = new List<byte>();
		public List<byte> vectornav_tlm_numGPS2Sats { get; set; } = new List<byte>();
		public List<byte> vectornav_tlm_GPS1Fix { get; set; } = new List<byte>();
		public List<byte> vectornav_tlm_GPS2Fix { get; set; } = new List<byte>();
		public List<ushort> vectornav_tlm_INSStatus { get; set; } = new List<ushort>();
		public List<ushort> vectornav_tlm_AHRSStatus { get; set; } = new List<ushort>();
		public List<float> vectornav_tlm_temp { get; set; } = new List<float>();
		public List<float> vectornav_tlm_press { get; set; } = new List<float>();
		public List<float> vectornav_tlm_Yaw { get; set; } = new List<float>();
		public List<float> vectornav_tlm_Pitch { get; set; } = new List<float>();
		public List<float> vectornav_tlm_Roll { get; set; } = new List<float>();
		public List<float> vectornav_tlm_linAcc_Bx { get; set; } = new List<float>();
		public List<float> vectornav_tlm_linAcc_By { get; set; } = new List<float>();
		public List<float> vectornav_tlm_linAcc_Bz { get; set; } = new List<float>();
		public List<float> prop_rpm { get; set; } = new List<float>();
		public List<float> battery_V { get; set; } = new List<float>();
		public List<float> fuel_percent { get; set; } = new List<float>();
		public List<float> meas_AGL { get; set; } = new List<float>();
		public List<uint> slow_status_pkt_errors { get; set; } = new List<uint>();
		public List<uint> slow_status_pkt_warnings { get; set; } = new List<uint>();
		public List<float> slow_status_pkt_battery_V { get; set; } = new List<float>();
		public List<float> slow_status_pkt_fuel_percent { get; set; } = new List<float>();
		public List<byte> slow_status_pkt_flagsA { get; set; } = new List<byte>();
		public List<byte> slow_status_pkt_power_status { get; set; } = new List<byte>();
		public List<byte> slow_status_pkt_flagsB { get; set; } = new List<byte>();
		public List<float> slow_status_pkt_battery_A { get; set; } = new List<float>();
		public List<float> slow_status_pkt_generator_V { get; set; } = new List<float>();
		public List<float> slow_status_pkt_generator_A { get; set; } = new List<float>();
		public List<uint> slow_status_pkt_slow_pkt_ctr { get; set; } = new List<uint>();
		public List<float> env_pkt_nose_airtemp { get; set; } = new List<float>();
		public List<float> env_pkt_fuel_flow { get; set; } = new List<float>();
		public List<float> env_pkt_est_fuel_wght { get; set; } = new List<float>();
		public List<float> env_pkt_eng_cyl1_temp { get; set; } = new List<float>();
		public List<float> env_pkt_eng_cyl2_temp { get; set; } = new List<float>();
		public List<ushort> env_pkt_valid { get; set; } = new List<ushort>();
		public List<float> pss8_pkt_impact_press { get; set; } = new List<float>();
		public List<float> pss8_pkt_static_press { get; set; } = new List<float>();
		public List<float> pss8_pkt_cal_airspeed { get; set; } = new List<float>();
		public List<float> pss8_pkt_true_airspeed { get; set; } = new List<float>();
		public List<float> pss8_pkt_press_altitude { get; set; } = new List<float>();
		public List<ushort> pss8_pkt_quality { get; set; } = new List<ushort>();
		public List<ushort> pss8_pkt_flags { get; set; } = new List<ushort>();
		public List<float> pss8_pkt_static_temp { get; set; } = new List<float>();
		public List<float> pss8_pkt_total_temp { get; set; } = new List<float>();
		public List<ushort> pss8_pkt_slow_quality { get; set; } = new List<ushort>();
		public List<ushort> pss8_pkt_slow_flags { get; set; } = new List<ushort>();
		public List<uint> pss8_pkt_pss8_pkt_ctr { get; set; } = new List<uint>();
		public List<uint> running_time { get; set; } = new List<uint>();
		public List<float> est_AOA { get; set; } = new List<float>();
		public int Size  = 664;

		public void Clear()

		{
			meas_w_B_ECI_B1.Clear();
			meas_w_B_ECI_B2.Clear();
			meas_w_B_ECI_B3.Clear();
			filt_w_B_ECI_B1.Clear();
			filt_w_B_ECI_B2.Clear();
			filt_w_B_ECI_B3.Clear();
			q_B_NED1.Clear();
			q_B_NED2.Clear();
			q_B_NED3.Clear();
			q_B_NED4.Clear();
			Euler321_B_NED_hs_psi_hs.Clear();
			Euler321_B_NED_hs_theta_hs.Clear();
			Euler321_B_NED_hs_phi_hs.Clear();
			Euler312_B_NED_hv_psi.Clear();
			Euler312_B_NED_hv_phi.Clear();
			Euler312_B_NED_hv_theta.Clear();
			filt_airspeed.Clear();
			airspeed_invalid.Clear();
			altimeter.Clear();
			est_alt.Clear();
			est_hdot.Clear();
			AGL_bias.Clear();
			time_unix.Clear();
			pss8_alt_invalid.Clear();
			Vel_B_ECF_NED1.Clear();
			Vel_B_ECF_NED2.Clear();
			Vel_B_ECF_NED3.Clear();
			sog.Clear();
			cog.Clear();
			rc_input_rc_aileron.Clear();
			rc_input_rc_elevator.Clear();
			rc_input_rc_flap.Clear();
			rc_input_rc_gear.Clear();
			rc_input_rc_rudder.Clear();
			rc_input_rc_throttle.Clear();
			rc_input_rc_man_bckup_lsb.Clear();
			rc_control_en.Clear();
			pos_NE1.Clear();
			pos_NE2.Clear();
			alt_INS.Clear();
			meas_lat.Clear();
			meas_lng.Clear();
			gps_ready.Clear();
			init_lat_lon1.Clear();
			init_lat_lon2.Clear();
			pos_initialized.Clear();
			Accel_NED1.Clear();
			Accel_NED2.Clear();
			Accel_NED3.Clear();
			est_pos_NE1.Clear();
			est_pos_NE2.Clear();
			est_vel_NE1.Clear();
			est_vel_NE2.Clear();
			Accel_B1.Clear();
			Accel_B2.Clear();
			Accel_B3.Clear();
			linux_time.Clear();
			INS_solution_valid.Clear();
			est_alt_bias.Clear();
			alt_limit_ctr.Clear();
			alt_meas_residual.Clear();
			vectornav_tlm_rawPositionLat.Clear();
			vectornav_tlm_rawPositionLon.Clear();
			vectornav_tlm_rawPositionAlt.Clear();
			vectornav_tlm_rawVel_N.Clear();
			vectornav_tlm_rawVel_E.Clear();
			vectornav_tlm_rawVel_D.Clear();
			vectornav_tlm_uncmpAngRate_Bx.Clear();
			vectornav_tlm_uncmpAngRate_By.Clear();
			vectornav_tlm_uncmpAngRate_Bz.Clear();
			vectornav_tlm_accel_Bx.Clear();
			vectornav_tlm_accel_By.Clear();
			vectornav_tlm_accel_Bz.Clear();
			vectornav_tlm_uncmpAccel_Bx.Clear();
			vectornav_tlm_uncmpAccel_By.Clear();
			vectornav_tlm_uncmpAccel_Bz.Clear();
			vectornav_tlm_mag_Bx.Clear();
			vectornav_tlm_mag_By.Clear();
			vectornav_tlm_mag_Bz.Clear();
			vectornav_tlm_sensSat.Clear();
			vectornav_tlm_yawU.Clear();
			vectornav_tlm_pitchU.Clear();
			vectornav_tlm_rollU.Clear();
			vectornav_tlm_posU.Clear();
			vectornav_tlm_velU.Clear();
			vectornav_tlm_GPS1GDOP.Clear();
			vectornav_tlm_GPS1TDOP.Clear();
			vectornav_tlm_GPS2GDOP.Clear();
			vectornav_tlm_numGPS1Sats.Clear();
			vectornav_tlm_numGPS2Sats.Clear();
			vectornav_tlm_GPS1Fix.Clear();
			vectornav_tlm_GPS2Fix.Clear();
			vectornav_tlm_INSStatus.Clear();
			vectornav_tlm_AHRSStatus.Clear();
			vectornav_tlm_temp.Clear();
			vectornav_tlm_press.Clear();
			vectornav_tlm_Yaw.Clear();
			vectornav_tlm_Pitch.Clear();
			vectornav_tlm_Roll.Clear();
			vectornav_tlm_linAcc_Bx.Clear();
			vectornav_tlm_linAcc_By.Clear();
			vectornav_tlm_linAcc_Bz.Clear();
			prop_rpm.Clear();
			battery_V.Clear();
			fuel_percent.Clear();
			meas_AGL.Clear();
			slow_status_pkt_errors.Clear();
			slow_status_pkt_warnings.Clear();
			slow_status_pkt_battery_V.Clear();
			slow_status_pkt_fuel_percent.Clear();
			slow_status_pkt_flagsA.Clear();
			slow_status_pkt_power_status.Clear();
			slow_status_pkt_flagsB.Clear();
			slow_status_pkt_battery_A.Clear();
			slow_status_pkt_generator_V.Clear();
			slow_status_pkt_generator_A.Clear();
			slow_status_pkt_slow_pkt_ctr.Clear();
			env_pkt_nose_airtemp.Clear();
			env_pkt_fuel_flow.Clear();
			env_pkt_est_fuel_wght.Clear();
			env_pkt_eng_cyl1_temp.Clear();
			env_pkt_eng_cyl2_temp.Clear();
			env_pkt_valid.Clear();
			pss8_pkt_impact_press.Clear();
			pss8_pkt_static_press.Clear();
			pss8_pkt_cal_airspeed.Clear();
			pss8_pkt_true_airspeed.Clear();
			pss8_pkt_press_altitude.Clear();
			pss8_pkt_quality.Clear();
			pss8_pkt_flags.Clear();
			pss8_pkt_static_temp.Clear();
			pss8_pkt_total_temp.Clear();
			pss8_pkt_slow_quality.Clear();
			pss8_pkt_slow_flags.Clear();
			pss8_pkt_pss8_pkt_ctr.Clear();
			running_time.Clear();
			est_AOA.Clear();
		}

		public void LogData(double uTime, double alt, double lat, double lon)
		{
			DateTime dt = UnixTimeStampToDateTime(uTime);

			using (StreamWriter w = File.AppendText("FAA_Data.txt"))
			{
				if ((int)alt > 0)
                {
                    if (OldDT.Second != dt.Second)
                    {
                        Debug.WriteLine(dt.TimeOfDay);
                        w.WriteLine($"{dt},{(int)alt},{R2D(lat)},{R2D(lon)}");
						using (StreamWriter f = File.AppendText("FAA_Data_lat_lon.txt"))
						{
							f.WriteLine($"{R2D(lat)},{R2D(lon)},{(int)alt}");
						};
							OldDT = dt;
                    }

                }
				
			}
		}
		public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
		{
			// Unix timestamp is seconds past epoch
			System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime() ;
			return dtDateTime;
		}
		public static DateTime UnixTimeToString(double unixTimeStamp)
		{
			// Unix timestamp is seconds past epoch
			System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
			dtDateTime = dtDateTime.AddTicks(-(dtDateTime.Ticks % TimeSpan.TicksPerSecond)); // removes milliseconds
			return dtDateTime;
		}
		public static double R2D(double radians)
		{
			double degrees = (180 / Math.PI) * radians;
			return (degrees);
		}
		public void ReadBytesData(Stream stream)

		{
			meas_w_B_ECI_B1.Add(Reader.ReadDouble(stream));
			meas_w_B_ECI_B2.Add(Reader.ReadDouble(stream));
			meas_w_B_ECI_B3.Add(Reader.ReadDouble(stream));
			filt_w_B_ECI_B1.Add(Reader.ReadDouble(stream));
			filt_w_B_ECI_B2.Add(Reader.ReadDouble(stream));
			filt_w_B_ECI_B3.Add(Reader.ReadDouble(stream));
			q_B_NED1.Add(Reader.ReadDouble(stream));
			q_B_NED2.Add(Reader.ReadDouble(stream));
			q_B_NED3.Add(Reader.ReadDouble(stream));
			q_B_NED4.Add(Reader.ReadDouble(stream));
			Euler321_B_NED_hs_psi_hs.Add(Reader.ReadDouble(stream));
			Euler321_B_NED_hs_theta_hs.Add(Reader.ReadDouble(stream));
			Euler321_B_NED_hs_phi_hs.Add(Reader.ReadDouble(stream));
			Euler312_B_NED_hv_psi.Add(Reader.ReadDouble(stream));
			Euler312_B_NED_hv_phi.Add(Reader.ReadDouble(stream));
			Euler312_B_NED_hv_theta.Add(Reader.ReadDouble(stream));
			filt_airspeed.Add(Reader.ReadDouble(stream));
			airspeed_invalid.Add(Reader.ReadSByte(stream));
			altimeter.Add(Reader.ReadDouble(stream));
			est_alt.Add(Reader.ReadDouble(stream));
			est_hdot.Add(Reader.ReadDouble(stream));
			AGL_bias.Add(Reader.ReadFloat(stream));
			time_unix.Add(Reader.ReadDouble(stream)); // get for FAA
			pss8_alt_invalid.Add(Reader.ReadSByte(stream));
			Vel_B_ECF_NED1.Add(Reader.ReadDouble(stream));
			Vel_B_ECF_NED2.Add(Reader.ReadDouble(stream));
			Vel_B_ECF_NED3.Add(Reader.ReadDouble(stream));
			sog.Add(Reader.ReadDouble(stream));
			cog.Add(Reader.ReadDouble(stream));
			rc_input_rc_aileron.Add(Reader.ReadDouble(stream));
			rc_input_rc_elevator.Add(Reader.ReadDouble(stream));
			rc_input_rc_flap.Add(Reader.ReadDouble(stream));
			rc_input_rc_gear.Add(Reader.ReadDouble(stream));
			rc_input_rc_rudder.Add(Reader.ReadDouble(stream));
			rc_input_rc_throttle.Add(Reader.ReadDouble(stream));
			rc_input_rc_man_bckup_lsb.Add(Reader.ReadShort(stream));
			rc_control_en.Add(Reader.ReadSByte(stream));
			pos_NE1.Add(Reader.ReadDouble(stream));
			pos_NE2.Add(Reader.ReadDouble(stream));
			alt_INS.Add(Reader.ReadDouble(stream)); // get for FAA
			meas_lat.Add(Reader.ReadDouble(stream)); // get for FAA
			meas_lng.Add(Reader.ReadDouble(stream)); // get for FAA
			gps_ready.Add(Reader.ReadSByte(stream));
			init_lat_lon1.Add(Reader.ReadDouble(stream));
			init_lat_lon2.Add(Reader.ReadDouble(stream));
			pos_initialized.Add(Reader.ReadSByte(stream));
			Accel_NED1.Add(Reader.ReadFloat(stream));
			Accel_NED2.Add(Reader.ReadFloat(stream));
			Accel_NED3.Add(Reader.ReadFloat(stream));
			est_pos_NE1.Add(Reader.ReadFloat(stream));
			est_pos_NE2.Add(Reader.ReadFloat(stream));
			est_vel_NE1.Add(Reader.ReadFloat(stream));
			est_vel_NE2.Add(Reader.ReadFloat(stream));
			Accel_B1.Add(Reader.ReadFloat(stream));
			Accel_B2.Add(Reader.ReadFloat(stream));
			Accel_B3.Add(Reader.ReadFloat(stream));
			linux_time.Add(Reader.ReadDouble(stream));
			INS_solution_valid.Add(Reader.ReadSByte(stream));
			est_alt_bias.Add(Reader.ReadDouble(stream));
			alt_limit_ctr.Add(Reader.ReadByte(stream));
			alt_meas_residual.Add(Reader.ReadFloat(stream));
			vectornav_tlm_rawPositionLat.Add(Reader.ReadDouble(stream));
			vectornav_tlm_rawPositionLon.Add(Reader.ReadDouble(stream));
			vectornav_tlm_rawPositionAlt.Add(Reader.ReadDouble(stream));
			vectornav_tlm_rawVel_N.Add(Reader.ReadFloat(stream));
			vectornav_tlm_rawVel_E.Add(Reader.ReadFloat(stream));
			vectornav_tlm_rawVel_D.Add(Reader.ReadFloat(stream));
			vectornav_tlm_uncmpAngRate_Bx.Add(Reader.ReadFloat(stream));
			vectornav_tlm_uncmpAngRate_By.Add(Reader.ReadFloat(stream));
			vectornav_tlm_uncmpAngRate_Bz.Add(Reader.ReadFloat(stream));
			vectornav_tlm_accel_Bx.Add(Reader.ReadFloat(stream));
			vectornav_tlm_accel_By.Add(Reader.ReadFloat(stream));
			vectornav_tlm_accel_Bz.Add(Reader.ReadFloat(stream));
			vectornav_tlm_uncmpAccel_Bx.Add(Reader.ReadFloat(stream));
			vectornav_tlm_uncmpAccel_By.Add(Reader.ReadFloat(stream));
			vectornav_tlm_uncmpAccel_Bz.Add(Reader.ReadFloat(stream));
			vectornav_tlm_mag_Bx.Add(Reader.ReadFloat(stream));
			vectornav_tlm_mag_By.Add(Reader.ReadFloat(stream));
			vectornav_tlm_mag_Bz.Add(Reader.ReadFloat(stream));
			vectornav_tlm_sensSat.Add(Reader.ReadUShort(stream));
			vectornav_tlm_yawU.Add(Reader.ReadFloat(stream));
			vectornav_tlm_pitchU.Add(Reader.ReadFloat(stream));
			vectornav_tlm_rollU.Add(Reader.ReadFloat(stream));
			vectornav_tlm_posU.Add(Reader.ReadFloat(stream));
			vectornav_tlm_velU.Add(Reader.ReadFloat(stream));
			vectornav_tlm_GPS1GDOP.Add(Reader.ReadFloat(stream));
			vectornav_tlm_GPS1TDOP.Add(Reader.ReadFloat(stream));
			vectornav_tlm_GPS2GDOP.Add(Reader.ReadFloat(stream));
			vectornav_tlm_numGPS1Sats.Add(Reader.ReadByte(stream));
			vectornav_tlm_numGPS2Sats.Add(Reader.ReadByte(stream));
			vectornav_tlm_GPS1Fix.Add(Reader.ReadByte(stream));
			vectornav_tlm_GPS2Fix.Add(Reader.ReadByte(stream));
			vectornav_tlm_INSStatus.Add(Reader.ReadUShort(stream));
			vectornav_tlm_AHRSStatus.Add(Reader.ReadUShort(stream));
			vectornav_tlm_temp.Add(Reader.ReadFloat(stream));
			vectornav_tlm_press.Add(Reader.ReadFloat(stream));
			vectornav_tlm_Yaw.Add(Reader.ReadFloat(stream));
			vectornav_tlm_Pitch.Add(Reader.ReadFloat(stream));
			vectornav_tlm_Roll.Add(Reader.ReadFloat(stream));
			vectornav_tlm_linAcc_Bx.Add(Reader.ReadFloat(stream));
			vectornav_tlm_linAcc_By.Add(Reader.ReadFloat(stream));
			vectornav_tlm_linAcc_Bz.Add(Reader.ReadFloat(stream));
			prop_rpm.Add(Reader.ReadFloat(stream));
			battery_V.Add(Reader.ReadFloat(stream));
			fuel_percent.Add(Reader.ReadFloat(stream));
			meas_AGL.Add(Reader.ReadFloat(stream));
			slow_status_pkt_errors.Add(Reader.ReadUInt(stream));
			slow_status_pkt_warnings.Add(Reader.ReadUInt(stream));
			slow_status_pkt_battery_V.Add(Reader.ReadFloat(stream));
			slow_status_pkt_fuel_percent.Add(Reader.ReadFloat(stream));
			slow_status_pkt_flagsA.Add(Reader.ReadByte(stream));
			slow_status_pkt_power_status.Add(Reader.ReadByte(stream));
			slow_status_pkt_flagsB.Add(Reader.ReadByte(stream));
			slow_status_pkt_battery_A.Add(Reader.ReadFloat(stream));
			slow_status_pkt_generator_V.Add(Reader.ReadFloat(stream));
			slow_status_pkt_generator_A.Add(Reader.ReadFloat(stream));
			slow_status_pkt_slow_pkt_ctr.Add(Reader.ReadUInt(stream));
			env_pkt_nose_airtemp.Add(Reader.ReadFloat(stream));
			env_pkt_fuel_flow.Add(Reader.ReadFloat(stream));
			env_pkt_est_fuel_wght.Add(Reader.ReadFloat(stream));
			env_pkt_eng_cyl1_temp.Add(Reader.ReadFloat(stream));
			env_pkt_eng_cyl2_temp.Add(Reader.ReadFloat(stream));
			env_pkt_valid.Add(Reader.ReadUShort(stream));
			pss8_pkt_impact_press.Add(Reader.ReadFloat(stream));
			pss8_pkt_static_press.Add(Reader.ReadFloat(stream));
			pss8_pkt_cal_airspeed.Add(Reader.ReadFloat(stream));
			pss8_pkt_true_airspeed.Add(Reader.ReadFloat(stream));
			pss8_pkt_press_altitude.Add(Reader.ReadFloat(stream));
			pss8_pkt_quality.Add(Reader.ReadUShort(stream));
			pss8_pkt_flags.Add(Reader.ReadUShort(stream));
			pss8_pkt_static_temp.Add(Reader.ReadFloat(stream));
			pss8_pkt_total_temp.Add(Reader.ReadFloat(stream));
			pss8_pkt_slow_quality.Add(Reader.ReadUShort(stream));
			pss8_pkt_slow_flags.Add(Reader.ReadUShort(stream));
			pss8_pkt_pss8_pkt_ctr.Add(Reader.ReadUInt(stream));
			running_time.Add(Reader.ReadUInt(stream));
			est_AOA.Add(Reader.ReadFloat(stream));
			//LogData(time_unix[time_unix.Count-1], alt_INS[alt_INS.Count - 1], meas_lat[meas_lat.Count - 1], meas_lng[meas_lng.Count - 1]);
		}
	}

	public class GuidData
	{
		public List<double> linux_time { get; set; } = new List<double>();
		public List<double> alt_rate_cmd_hs { get; set; } = new List<double>();
		public List<double> phi_cmd_hs { get; set; } = new List<double>();
		public List<double> speed_cmd_hs { get; set; } = new List<double>();
		public List<double> alt_cmd_hs { get; set; } = new List<double>();
		public List<double> cog_cmd_hs { get; set; } = new List<double>();
		public List<sbyte> increment_wpt { get; set; } = new List<sbyte>();
		public List<double> psi_cmd { get; set; } = new List<double>();
		public List<double> urate { get; set; } = new List<double>();
		public List<double> pos_cmd_NE1 { get; set; } = new List<double>();
		public List<double> pos_cmd_NE2 { get; set; } = new List<double>();
		public List<double> alt_cmd { get; set; } = new List<double>();
		public List<double> hv_wpt_err { get; set; } = new List<double>();
		public List<double> vel_cmd_NED1 { get; set; } = new List<double>();
		public List<double> vel_cmd_NED2 { get; set; } = new List<double>();
		public List<double> vel_cmd_NED3 { get; set; } = new List<double>();
		public List<float> cl_hv { get; set; } = new List<float>();
		public List<byte> stick_mode { get; set; } = new List<byte>();
		public List<sbyte> flip_state { get; set; } = new List<sbyte>();
		public List<float> wind_est_NE1 { get; set; } = new List<float>();
		public List<float> wind_est_NE2 { get; set; } = new List<float>();
		public List<float> hdg_err { get; set; } = new List<float>();
		public List<byte> mvb_land_state { get; set; } = new List<byte>();
		public List<sbyte> hover_done { get; set; } = new List<sbyte>();
		public List<sbyte> launch_done { get; set; } = new List<sbyte>();
		public List<ushort> loiter_countdown { get; set; } = new List<ushort>();
		public List<ushort> waypoint_eta { get; set; } = new List<ushort>();
		public List<double> phi_cmd_trans { get; set; } = new List<double>();
		public List<double> theta_cmd_trans { get; set; } = new List<double>();
		public List<double> hdg_cmd_trans { get; set; } = new List<double>();
		public List<double> airspeed_cmd_trans { get; set; } = new List<double>();
		public int Size  = 178;

		public void Clear()

		{
			linux_time.Clear();
			alt_rate_cmd_hs.Clear();
			phi_cmd_hs.Clear();
			speed_cmd_hs.Clear();
			alt_cmd_hs.Clear();
			cog_cmd_hs.Clear();
			increment_wpt.Clear();
			psi_cmd.Clear();
			urate.Clear();
			pos_cmd_NE1.Clear();
			pos_cmd_NE2.Clear();
			alt_cmd.Clear();
			hv_wpt_err.Clear();
			vel_cmd_NED1.Clear();
			vel_cmd_NED2.Clear();
			vel_cmd_NED3.Clear();
			cl_hv.Clear();
			stick_mode.Clear();
			flip_state.Clear();
			wind_est_NE1.Clear();
			wind_est_NE2.Clear();
			hdg_err.Clear();
			mvb_land_state.Clear();
			hover_done.Clear();
			launch_done.Clear();
			loiter_countdown.Clear();
			waypoint_eta.Clear();
			phi_cmd_trans.Clear();
			theta_cmd_trans.Clear();
			hdg_cmd_trans.Clear();
			airspeed_cmd_trans.Clear();
		}

		public void ReadBytesData(Stream stream)

		{
			linux_time.Add(Reader.ReadDouble(stream));
			alt_rate_cmd_hs.Add(Reader.ReadDouble(stream));
			phi_cmd_hs.Add(Reader.ReadDouble(stream));
			speed_cmd_hs.Add(Reader.ReadDouble(stream));
			alt_cmd_hs.Add(Reader.ReadDouble(stream));
			cog_cmd_hs.Add(Reader.ReadDouble(stream));
			increment_wpt.Add(Reader.ReadSByte(stream));
			psi_cmd.Add(Reader.ReadDouble(stream));
			urate.Add(Reader.ReadDouble(stream));
			pos_cmd_NE1.Add(Reader.ReadDouble(stream));
			pos_cmd_NE2.Add(Reader.ReadDouble(stream));
			alt_cmd.Add(Reader.ReadDouble(stream));
			hv_wpt_err.Add(Reader.ReadDouble(stream));
			vel_cmd_NED1.Add(Reader.ReadDouble(stream));
			vel_cmd_NED2.Add(Reader.ReadDouble(stream));
			vel_cmd_NED3.Add(Reader.ReadDouble(stream));
			cl_hv.Add(Reader.ReadFloat(stream));
			stick_mode.Add(Reader.ReadByte(stream));
			flip_state.Add(Reader.ReadSByte(stream));
			wind_est_NE1.Add(Reader.ReadFloat(stream));
			wind_est_NE2.Add(Reader.ReadFloat(stream));
			hdg_err.Add(Reader.ReadFloat(stream));
			mvb_land_state.Add(Reader.ReadByte(stream));
			hover_done.Add(Reader.ReadSByte(stream));
			launch_done.Add(Reader.ReadSByte(stream));
			loiter_countdown.Add(Reader.ReadUShort(stream));
			waypoint_eta.Add(Reader.ReadUShort(stream));
			phi_cmd_trans.Add(Reader.ReadDouble(stream));
			theta_cmd_trans.Add(Reader.ReadDouble(stream));
			hdg_cmd_trans.Add(Reader.ReadDouble(stream));
			airspeed_cmd_trans.Add(Reader.ReadDouble(stream));
		}
	}

	public class ModeData
	{
		public List<double> linux_time { get; set; } = new List<double>();
		public List<byte> flight_state_tlm { get; set; } = new List<byte>();
		public List<byte> last_flight_state_tlm { get; set; } = new List<byte>();
		public List<sbyte> control_mode { get; set; } = new List<sbyte>();
		public List<sbyte> start_waypoint_mode { get; set; } = new List<sbyte>();
		public List<short> next_wpt { get; set; } = new List<short>();
		public List<byte> flightplan { get; set; } = new List<byte>();
		public List<sbyte> wpt_changed { get; set; } = new List<sbyte>();
		public List<sbyte> prevent_increment { get; set; } = new List<sbyte>();
		public List<sbyte> auto_landing { get; set; } = new List<sbyte>();
		public List<byte> nav_light_power { get; set; } = new List<byte>();
		public List<sbyte> rpm_low { get; set; } = new List<sbyte>();
		public int Size  = 20;

		public void Clear()

		{
			linux_time.Clear();
			flight_state_tlm.Clear();
			last_flight_state_tlm.Clear();
			control_mode.Clear();
			start_waypoint_mode.Clear();
			next_wpt.Clear();
			flightplan.Clear();
			wpt_changed.Clear();
			prevent_increment.Clear();
			auto_landing.Clear();
			nav_light_power.Clear();
			rpm_low.Clear();
		}

		public void ReadBytesData(Stream stream)

		{
			linux_time.Add(Reader.ReadDouble(stream));
			flight_state_tlm.Add(Reader.ReadByte(stream));
			last_flight_state_tlm.Add(Reader.ReadByte(stream));
			control_mode.Add(Reader.ReadSByte(stream));
			start_waypoint_mode.Add(Reader.ReadSByte(stream));
			next_wpt.Add(Reader.ReadShort(stream));
			flightplan.Add(Reader.ReadByte(stream));
			wpt_changed.Add(Reader.ReadSByte(stream));
			prevent_increment.Add(Reader.ReadSByte(stream));
			auto_landing.Add(Reader.ReadSByte(stream));
			nav_light_power.Add(Reader.ReadByte(stream));
			rpm_low.Add(Reader.ReadSByte(stream));
		}
	}

	public class GCSdData
	{
		public List<double> linux_time { get; set; } = new List<double>();
		public List<sbyte> shield_command { get; set; } = new List<sbyte>();
		public List<float> c2_ack_ratio { get; set; } = new List<float>();
		public List<byte> mvg_base_HDOP { get; set; } = new List<byte>();
		public List<double> est_pos_mvb_NE1 { get; set; } = new List<double>();
		public List<double> est_pos_mvb_NE2 { get; set; } = new List<double>();
		public List<double> delta_X { get; set; } = new List<double>();
		public List<double> delta_Y { get; set; } = new List<double>();
		public List<int> accum_mvb_upd { get; set; } = new List<int>();
		public List<double> est_vel_mvb_NE1 { get; set; } = new List<double>();
		public List<double> est_vel_mvb_NE2 { get; set; } = new List<double>();
		public List<sbyte> mvb_en { get; set; } = new List<sbyte>();
		public List<float> meas_pos_mvb_NE1 { get; set; } = new List<float>();
		public List<float> meas_pos_mvb_NE2 { get; set; } = new List<float>();
		public List<float> meas_vel_mvb_NE1 { get; set; } = new List<float>();
		public List<float> meas_vel_mvb_NE2 { get; set; } = new List<float>();
		public List<byte> fp_upload_cnt { get; set; } = new List<byte>();
		public List<short> fltplan_num_wpt1 { get; set; } = new List<short>();
		public List<short> fltplan_num_wpt2 { get; set; } = new List<short>();
		public List<short> fltplan_num_wpt3 { get; set; } = new List<short>();
		public List<short> fltplan_num_wpt4 { get; set; } = new List<short>();
		public List<short> fltplan_num_wpt5 { get; set; } = new List<short>();
		public List<short> fltplan_num_wpt6 { get; set; } = new List<short>();
		public List<short> fltplan_num_wpt7 { get; set; } = new List<short>();
		public List<sbyte> current_fp_changed { get; set; } = new List<sbyte>();
		public List<short> next_waypoint { get; set; } = new List<short>();
		public List<sbyte> next_wpt_updated { get; set; } = new List<sbyte>();
		public List<byte> packet20_freq { get; set; } = new List<byte>();
		public List<byte> xbus_freq { get; set; } = new List<byte>();
		public List<sbyte> lost_comm_fault_HV { get; set; } = new List<sbyte>();
		public List<sbyte> lost_comm_fault_HS { get; set; } = new List<sbyte>();
		public List<sbyte> stick_input_throttle_stick_lsb { get; set; } = new List<sbyte>();
		public List<sbyte> stick_input_aileron_stick_lsb { get; set; } = new List<sbyte>();
		public List<sbyte> stick_input_elevator_stick_lsb { get; set; } = new List<sbyte>();
		public List<sbyte> stick_input_rudder_stick_lsb { get; set; } = new List<sbyte>();
		public List<sbyte> stick_input_aux_A_stick_lsb { get; set; } = new List<sbyte>();
		public List<sbyte> stick_input_aux_B_stick_lsb { get; set; } = new List<sbyte>();
		public List<sbyte> stick_input_engine_disable { get; set; } = new List<sbyte>();
		public List<sbyte> stick_input_control_mode { get; set; } = new List<sbyte>();
		public List<sbyte> control_mode_rcvd { get; set; } = new List<sbyte>();
		public List<sbyte> auto_manvr_reset { get; set; } = new List<sbyte>();
		public List<sbyte> loss_of_xbus_fault { get; set; } = new List<sbyte>();
		public List<sbyte> constant_bank_en { get; set; } = new List<sbyte>();
		public int Size  = 118;

		public void Clear()

		{
			linux_time.Clear();
			shield_command.Clear();
			c2_ack_ratio.Clear();
			mvg_base_HDOP.Clear();
			est_pos_mvb_NE1.Clear();
			est_pos_mvb_NE2.Clear();
			delta_X.Clear();
			delta_Y.Clear();
			accum_mvb_upd.Clear();
			est_vel_mvb_NE1.Clear();
			est_vel_mvb_NE2.Clear();
			mvb_en.Clear();
			meas_pos_mvb_NE1.Clear();
			meas_pos_mvb_NE2.Clear();
			meas_vel_mvb_NE1.Clear();
			meas_vel_mvb_NE2.Clear();
			fp_upload_cnt.Clear();
			fltplan_num_wpt1.Clear();
			fltplan_num_wpt2.Clear();
			fltplan_num_wpt3.Clear();
			fltplan_num_wpt4.Clear();
			fltplan_num_wpt5.Clear();
			fltplan_num_wpt6.Clear();
			fltplan_num_wpt7.Clear();
			current_fp_changed.Clear();
			next_waypoint.Clear();
			next_wpt_updated.Clear();
			packet20_freq.Clear();
			xbus_freq.Clear();
			lost_comm_fault_HV.Clear();
			lost_comm_fault_HS.Clear();
			stick_input_throttle_stick_lsb.Clear();
			stick_input_aileron_stick_lsb.Clear();
			stick_input_elevator_stick_lsb.Clear();
			stick_input_rudder_stick_lsb.Clear();
			stick_input_aux_A_stick_lsb.Clear();
			stick_input_aux_B_stick_lsb.Clear();
			stick_input_engine_disable.Clear();
			stick_input_control_mode.Clear();
			control_mode_rcvd.Clear();
			auto_manvr_reset.Clear();
			loss_of_xbus_fault.Clear();
			constant_bank_en.Clear();
		}

		public void ReadBytesData(Stream stream)

		{
			linux_time.Add(Reader.ReadDouble(stream));
			shield_command.Add(Reader.ReadSByte(stream));
			c2_ack_ratio.Add(Reader.ReadFloat(stream));
			mvg_base_HDOP.Add(Reader.ReadByte(stream));
			est_pos_mvb_NE1.Add(Reader.ReadDouble(stream));
			est_pos_mvb_NE2.Add(Reader.ReadDouble(stream));
			delta_X.Add(Reader.ReadDouble(stream));
			delta_Y.Add(Reader.ReadDouble(stream));
			accum_mvb_upd.Add(Reader.ReadInt(stream));
			est_vel_mvb_NE1.Add(Reader.ReadDouble(stream));
			est_vel_mvb_NE2.Add(Reader.ReadDouble(stream));
			mvb_en.Add(Reader.ReadSByte(stream));
			meas_pos_mvb_NE1.Add(Reader.ReadFloat(stream));
			meas_pos_mvb_NE2.Add(Reader.ReadFloat(stream));
			meas_vel_mvb_NE1.Add(Reader.ReadFloat(stream));
			meas_vel_mvb_NE2.Add(Reader.ReadFloat(stream));
			fp_upload_cnt.Add(Reader.ReadByte(stream));
			fltplan_num_wpt1.Add(Reader.ReadShort(stream));
			fltplan_num_wpt2.Add(Reader.ReadShort(stream));
			fltplan_num_wpt3.Add(Reader.ReadShort(stream));
			fltplan_num_wpt4.Add(Reader.ReadShort(stream));
			fltplan_num_wpt5.Add(Reader.ReadShort(stream));
			fltplan_num_wpt6.Add(Reader.ReadShort(stream));
			fltplan_num_wpt7.Add(Reader.ReadShort(stream));
			current_fp_changed.Add(Reader.ReadSByte(stream));
			next_waypoint.Add(Reader.ReadShort(stream));
			next_wpt_updated.Add(Reader.ReadSByte(stream));
			packet20_freq.Add(Reader.ReadByte(stream));
			xbus_freq.Add(Reader.ReadByte(stream));
			lost_comm_fault_HV.Add(Reader.ReadSByte(stream));
			lost_comm_fault_HS.Add(Reader.ReadSByte(stream));
			stick_input_throttle_stick_lsb.Add(Reader.ReadSByte(stream));
			stick_input_aileron_stick_lsb.Add(Reader.ReadSByte(stream));
			stick_input_elevator_stick_lsb.Add(Reader.ReadSByte(stream));
			stick_input_rudder_stick_lsb.Add(Reader.ReadSByte(stream));
			stick_input_aux_A_stick_lsb.Add(Reader.ReadSByte(stream));
			stick_input_aux_B_stick_lsb.Add(Reader.ReadSByte(stream));
			stick_input_engine_disable.Add(Reader.ReadSByte(stream));
			stick_input_control_mode.Add(Reader.ReadSByte(stream));
			control_mode_rcvd.Add(Reader.ReadSByte(stream));
			auto_manvr_reset.Add(Reader.ReadSByte(stream));
			loss_of_xbus_fault.Add(Reader.ReadSByte(stream));
			constant_bank_en.Add(Reader.ReadSByte(stream));
		}
	}

	public class CtrlData
	{
		public List<double> linux_time { get; set; } = new List<double>();
		public List<double> elevator { get; set; } = new List<double>();
		public List<double> throttle { get; set; } = new List<double>();
		public List<double> rudder { get; set; } = new List<double>();
		public List<double> aileron { get; set; } = new List<double>();
		public List<double> roll { get; set; } = new List<double>();
		public List<double> alt_error { get; set; } = new List<double>();
		public List<double> vel_err_Prime1 { get; set; } = new List<double>();
		public List<double> vel_err_Prime2 { get; set; } = new List<double>();
		public List<double> pos_err_Prime1 { get; set; } = new List<double>();
		public List<double> pos_err_Prime2 { get; set; } = new List<double>();
		public List<double> integ_alt_err { get; set; } = new List<double>();
		public List<double> theta_cmd { get; set; } = new List<double>();
		public List<double> phi_cmd { get; set; } = new List<double>();
		public List<double> hv_att_error1 { get; set; } = new List<double>();
		public List<double> hv_att_error2 { get; set; } = new List<double>();
		public List<double> hv_att_error3 { get; set; } = new List<double>();
		public List<double> hv_int_psi_error { get; set; } = new List<double>();
		public List<double> hv_int_hdot_error { get; set; } = new List<double>();
		public List<float> speed_err_int { get; set; } = new List<float>();
		public List<float> attHv_integ1 { get; set; } = new List<float>();
		public List<float> attHv_integ2 { get; set; } = new List<float>();
		public List<float> attHv_integ3 { get; set; } = new List<float>();
		public List<byte> saturate_ctr { get; set; } = new List<byte>();
		public int Size  = 169;

		public void Clear()

		{
			linux_time.Clear();
			elevator.Clear();
			throttle.Clear();
			rudder.Clear();
			aileron.Clear();
			roll.Clear();
			alt_error.Clear();
			vel_err_Prime1.Clear();
			vel_err_Prime2.Clear();
			pos_err_Prime1.Clear();
			pos_err_Prime2.Clear();
			integ_alt_err.Clear();
			theta_cmd.Clear();
			phi_cmd.Clear();
			hv_att_error1.Clear();
			hv_att_error2.Clear();
			hv_att_error3.Clear();
			hv_int_psi_error.Clear();
			hv_int_hdot_error.Clear();
			speed_err_int.Clear();
			attHv_integ1.Clear();
			attHv_integ2.Clear();
			attHv_integ3.Clear();
			saturate_ctr.Clear();
		}

		public void ReadBytesData(Stream stream)

		{
			linux_time.Add(Reader.ReadDouble(stream));
			elevator.Add(Reader.ReadDouble(stream));
			throttle.Add(Reader.ReadDouble(stream));
			rudder.Add(Reader.ReadDouble(stream));
			aileron.Add(Reader.ReadDouble(stream));
			roll.Add(Reader.ReadDouble(stream));
			alt_error.Add(Reader.ReadDouble(stream));
			vel_err_Prime1.Add(Reader.ReadDouble(stream));
			vel_err_Prime2.Add(Reader.ReadDouble(stream));
			pos_err_Prime1.Add(Reader.ReadDouble(stream));
			pos_err_Prime2.Add(Reader.ReadDouble(stream));
			integ_alt_err.Add(Reader.ReadDouble(stream));
			theta_cmd.Add(Reader.ReadDouble(stream));
			phi_cmd.Add(Reader.ReadDouble(stream));
			hv_att_error1.Add(Reader.ReadDouble(stream));
			hv_att_error2.Add(Reader.ReadDouble(stream));
			hv_att_error3.Add(Reader.ReadDouble(stream));
			hv_int_psi_error.Add(Reader.ReadDouble(stream));
			hv_int_hdot_error.Add(Reader.ReadDouble(stream));
			speed_err_int.Add(Reader.ReadFloat(stream));
			attHv_integ1.Add(Reader.ReadFloat(stream));
			attHv_integ2.Add(Reader.ReadFloat(stream));
			attHv_integ3.Add(Reader.ReadFloat(stream));
			saturate_ctr.Add(Reader.ReadByte(stream));
		}
	}

	public class NavdData
	{
		public List<double> linux_time { get; set; } = new List<double>();
		public List<float> est_V_wind_N { get; set; } = new List<float>();
		public List<float> est_V_wind_E { get; set; } = new List<float>();
		public List<float> P11_V_wind_N { get; set; } = new List<float>();
		public List<float> P22_V_wind_E { get; set; } = new List<float>();
		public List<float> P33_IAS_bias { get; set; } = new List<float>();
		public List<float> TAS_meas_residual { get; set; } = new List<float>();
		public List<float> rho { get; set; } = new List<float>();
		public List<float> est_IAS_bias { get; set; } = new List<float>();
		public List<float> est_wind_dir { get; set; } = new List<float>();
		public List<float> est_wind_speed { get; set; } = new List<float>();
		public List<float> density_alt { get; set; } = new List<float>();
		public List<uint> tag_age_body { get; set; } = new List<uint>();
		public List<float> tag_pos_B_T_B1 { get; set; } = new List<float>();
		public List<float> tag_pos_B_T_B2 { get; set; } = new List<float>();
		public List<float> tag_pos_B_T_B3 { get; set; } = new List<float>();
		public List<uint> tag_age_ned { get; set; } = new List<uint>();
		public List<float> tag_pos_B_T_NED1 { get; set; } = new List<float>();
		public List<float> tag_pos_B_T_NED2 { get; set; } = new List<float>();
		public List<float> tag_pos_B_T_NED3 { get; set; } = new List<float>();
		public List<float> tag_vel_T_B_NED1 { get; set; } = new List<float>();
		public List<float> tag_vel_T_B_NED2 { get; set; } = new List<float>();
		public List<float> tag_vel_T_B_NED3 { get; set; } = new List<float>();
		public List<int> accum_body_upd { get; set; } = new List<int>();
		public List<int> accum_ned_upd { get; set; } = new List<int>();
		public List<sbyte> tracking { get; set; } = new List<sbyte>();
		public List<double> pos_B_T_NED1 { get; set; } = new List<double>();
		public List<double> pos_B_T_NED2 { get; set; } = new List<double>();
		public List<double> pos_B_T_NED3 { get; set; } = new List<double>();
		public List<double> vel_T_B_NED1 { get; set; } = new List<double>();
		public List<double> vel_T_B_NED2 { get; set; } = new List<double>();
		public List<double> vel_T_B_NED3 { get; set; } = new List<double>();
		public List<float> est_pos_B_T_NED1 { get; set; } = new List<float>();
		public List<float> est_pos_B_T_NED2 { get; set; } = new List<float>();
		public List<float> est_pos_B_T_NED3 { get; set; } = new List<float>();
		public List<float> est_vel_T_B_NED1 { get; set; } = new List<float>();
		public List<float> est_vel_T_B_NED2 { get; set; } = new List<float>();
		public List<float> est_vel_T_B_NED3 { get; set; } = new List<float>();
		public List<sbyte> altitude_low { get; set; } = new List<sbyte>();
		public List<float> V_min { get; set; } = new List<float>();
		public List<float> est_mass_lbm { get; set; } = new List<float>();
		public int Size  = 186;

		public void Clear()

		{
			linux_time.Clear();
			est_V_wind_N.Clear();
			est_V_wind_E.Clear();
			P11_V_wind_N.Clear();
			P22_V_wind_E.Clear();
			P33_IAS_bias.Clear();
			TAS_meas_residual.Clear();
			rho.Clear();
			est_IAS_bias.Clear();
			est_wind_dir.Clear();
			est_wind_speed.Clear();
			density_alt.Clear();
			tag_age_body.Clear();
			tag_pos_B_T_B1.Clear();
			tag_pos_B_T_B2.Clear();
			tag_pos_B_T_B3.Clear();
			tag_age_ned.Clear();
			tag_pos_B_T_NED1.Clear();
			tag_pos_B_T_NED2.Clear();
			tag_pos_B_T_NED3.Clear();
			tag_vel_T_B_NED1.Clear();
			tag_vel_T_B_NED2.Clear();
			tag_vel_T_B_NED3.Clear();
			accum_body_upd.Clear();
			accum_ned_upd.Clear();
			tracking.Clear();
			pos_B_T_NED1.Clear();
			pos_B_T_NED2.Clear();
			pos_B_T_NED3.Clear();
			vel_T_B_NED1.Clear();
			vel_T_B_NED2.Clear();
			vel_T_B_NED3.Clear();
			est_pos_B_T_NED1.Clear();
			est_pos_B_T_NED2.Clear();
			est_pos_B_T_NED3.Clear();
			est_vel_T_B_NED1.Clear();
			est_vel_T_B_NED2.Clear();
			est_vel_T_B_NED3.Clear();
			altitude_low.Clear();
			V_min.Clear();
			est_mass_lbm.Clear();
		}

		public void ReadBytesData(Stream stream)

		{
			linux_time.Add(Reader.ReadDouble(stream));
			est_V_wind_N.Add(Reader.ReadFloat(stream));
			est_V_wind_E.Add(Reader.ReadFloat(stream));
			P11_V_wind_N.Add(Reader.ReadFloat(stream));
			P22_V_wind_E.Add(Reader.ReadFloat(stream));
			P33_IAS_bias.Add(Reader.ReadFloat(stream));
			TAS_meas_residual.Add(Reader.ReadFloat(stream));
			rho.Add(Reader.ReadFloat(stream));
			est_IAS_bias.Add(Reader.ReadFloat(stream));
			est_wind_dir.Add(Reader.ReadFloat(stream));
			est_wind_speed.Add(Reader.ReadFloat(stream));
			density_alt.Add(Reader.ReadFloat(stream));
			tag_age_body.Add(Reader.ReadUInt(stream));
			tag_pos_B_T_B1.Add(Reader.ReadFloat(stream));
			tag_pos_B_T_B2.Add(Reader.ReadFloat(stream));
			tag_pos_B_T_B3.Add(Reader.ReadFloat(stream));
			tag_age_ned.Add(Reader.ReadUInt(stream));
			tag_pos_B_T_NED1.Add(Reader.ReadFloat(stream));
			tag_pos_B_T_NED2.Add(Reader.ReadFloat(stream));
			tag_pos_B_T_NED3.Add(Reader.ReadFloat(stream));
			tag_vel_T_B_NED1.Add(Reader.ReadFloat(stream));
			tag_vel_T_B_NED2.Add(Reader.ReadFloat(stream));
			tag_vel_T_B_NED3.Add(Reader.ReadFloat(stream));
			accum_body_upd.Add(Reader.ReadInt(stream));
			accum_ned_upd.Add(Reader.ReadInt(stream));
			tracking.Add(Reader.ReadSByte(stream));
			pos_B_T_NED1.Add(Reader.ReadDouble(stream));
			pos_B_T_NED2.Add(Reader.ReadDouble(stream));
			pos_B_T_NED3.Add(Reader.ReadDouble(stream));
			vel_T_B_NED1.Add(Reader.ReadDouble(stream));
			vel_T_B_NED2.Add(Reader.ReadDouble(stream));
			vel_T_B_NED3.Add(Reader.ReadDouble(stream));
			est_pos_B_T_NED1.Add(Reader.ReadFloat(stream));
			est_pos_B_T_NED2.Add(Reader.ReadFloat(stream));
			est_pos_B_T_NED3.Add(Reader.ReadFloat(stream));
			est_vel_T_B_NED1.Add(Reader.ReadFloat(stream));
			est_vel_T_B_NED2.Add(Reader.ReadFloat(stream));
			est_vel_T_B_NED3.Add(Reader.ReadFloat(stream));
			altitude_low.Add(Reader.ReadSByte(stream));
			V_min.Add(Reader.ReadFloat(stream));
			est_mass_lbm.Add(Reader.ReadFloat(stream));
		}
	}

	public static class Reader
	{
		public static double ReadDouble(Stream stream)
		{
			var rawBytes = new byte[8];
			stream.Read(rawBytes, 0, 8);
			//Debug.WriteLine(BitConverter.ToDouble(rawBytes, 0));
			return BitConverter.ToDouble(rawBytes, 0);
		}
		public static float ReadFloat(Stream stream)
		{
			var rawBytes = new byte[4];
			stream.Read(rawBytes, 0, 4);
			//Debug.WriteLine(BitConverter.ToDouble(rawBytes, 0));
			return BitConverter.ToSingle(rawBytes, 0);
		}
		public static byte ReadByte(Stream stream)
		{
			// uint8
			byte[] rawBytes = new byte[1];
			stream.Read(rawBytes, 0, 1);
			return rawBytes[0];
		}
		public static sbyte ReadSByte(Stream stream)
		{
			// int8
			byte[] rawBytes = new byte[1];
			stream.Read(rawBytes, 0, 1);
			return (sbyte)rawBytes[0];
		}
		public static Int16 ReadShort(Stream stream)
		{
			var rawBytes = new byte[2];
			stream.Read(rawBytes, 0, 2);
			return BitConverter.ToInt16(rawBytes, 0);
		}
		public static Int32 ReadInt(Stream stream)
		{
			var rawBytes = new byte[4];
			stream.Read(rawBytes, 0, 4);
			return BitConverter.ToInt32(rawBytes, 0);
		}
		public static UInt32 ReadUInt(Stream stream)
		{
			var rawBytes = new byte[4];
			stream.Read(rawBytes, 0, 4);
			return BitConverter.ToUInt32(rawBytes, 0);
		}
		public static UInt16 ReadUShort(Stream stream)
		{
			var rawBytes = new byte[2];
			stream.Read(rawBytes, 0, 2);
			return BitConverter.ToUInt16(rawBytes, 0);
		}
		public static bool ReadBool(Stream stream)
		{
			byte[] rawBytes = new byte[1];
			stream.Read(rawBytes, 0, 1);
			return Convert.ToBoolean((sbyte)rawBytes[0]);
		}
		public static string ReadLogValue(Stream stream, Int32 len)
		{
			string rb = string.Empty;
			var rawBytes = new byte[len];
			stream.Read(rawBytes, 0, len);
			return rb = Encoding.ASCII.GetString(rawBytes, 0, len);
        }
        

    }


	public class ParseTLV
	{
		public int snsrLen = 650; // 658 length+tag+len// length of payload in bytes 
		public int guidLen = 142; // 150
		public int modeLen = 18;  // 026
		public int gcsdLen = 118; // 126
		public int ctrlLen = 160; // 168
		public int navdLen = 186; // 194

		public int snsrLen17 = 650; // 658 length+tag+len// length of payload in bytes 
		public int guidLen17 = 146; // 154
		public int modeLen17 = 19;  // 027
		public int gcsdLen17 = 118; // 126
		public int ctrlLen17 = 161; // 169
		public int navdLen17 = 186; // 194

		public int LogLinelength = 1322; //total len bytes per composite packet
		public int LogLinelength17 = 1328; //total len bytes per composite packet
		byte[] payload = new byte[1];
		const int TagLen = 4;
		const int LengthLen = 4;
		List<int> LengthList { get; set; }
		List<int> LengthList17 { get; set; }

		public Dictionary<string, int> TLVDict { get; set; }
		public Dictionary<string, int> TLVDict17 { get; set; }

		public ParseTLV()
		{
			// carried over from python
			// d = double 8  double
			// f = Float  4  float
			// ? = bool   1  bool
			// h = short  2  short
			// H = ushort 2  ushort
			// B = uChar  1  byte
			// b = char   1  sbyte
			// i = int    4  int
			// I = uInt   4  uint

			LengthList = new List<int>() { snsrLen, guidLen, modeLen, gcsdLen, ctrlLen, navdLen };
			LengthList17 = new List<int>() { snsrLen17, guidLen17, modeLen17, gcsdLen17, ctrlLen17, navdLen17 };
			TLVDict = new Dictionary<string, int>
			{
				{"Snsr",snsrLen },
				{"Guid",guidLen },
				{"Mode",modeLen },
				{"Gcsd",gcsdLen },
				{"Ctrl",ctrlLen },
				{"Navd",navdLen }
			};

			TLVDict17 = new Dictionary<string, int>
			{
				{"Snsr",snsrLen17 },
				{"Guid",guidLen17 },
				{"Mode",modeLen17 },
				{"Gcsd",gcsdLen17 },
				{"Ctrl",ctrlLen17 },
				{"Navd",navdLen17 }
			};
		}

		public void DemuxToMemory(string path, ProgressBar p, Label l, string version)
		{
			Debug.WriteLine($"Version : {version}");

			int completePct = 0;
			//int LineCount = 0;
			using (var stream = File.OpenRead(path))
			{
				
				Debug.WriteLine($"v171 Log is {stream.Length} bytes");
				byte[] LogLines = new byte[stream.Length / LogLinelength17];
				p.Maximum = 100;
				int pctNow = 0;
				int oldPct = 0;
				while (stream.Length != stream.Position)
				{

					float pctComp = ((float)stream.Position / (float)stream.Length) * 100;
					pctNow = (int)pctComp;
					if (pctNow % 2 == 0 && pctNow != oldPct)
					{

						p.Value += 2;
						completePct += 2;
						l.Text = $"{pctNow} %";
						l.Update();
						//Debug.WriteLine(pctNow);
						oldPct = pctNow;
					}

					//FillClasses(stream, S17, G117, M17, G217, C17, N17);
					FillClasses(stream);
				}
			}
			l.Text = string.Empty;
			//LineCount = 0;
		}

		public void FillClasses(Stream stream)
		{

			string Tag = ReadTag(stream);
			Int32 l = ReadLength(stream);
			//Debug.WriteLine($"Tag: {Tag} Len: {l}");
			//ReadValue(stream, l);
			switch (Tag)
			{
				case "SESS":
					Form1.Sess171.ReadData(l, stream);
					break;
				case "META":
					Form1.Meta171.ReadData(l, stream);
					break;
				case "Alog":
					Form1.Alog171.ReadData(l, stream);
					break;
				case "Clog":
					Form1.Clog171.ReadData(l, stream);
					break;
				case "Snsr":
					Form1.Snsr171.ReadBytesData(stream);
					break;
				case "Guid":
					Form1.Guid171.ReadBytesData(stream);
					break;
				case "Mode":
					Form1.Mode171.ReadBytesData(stream);
					break;
				case "GCSd":
					Form1.GCS171.ReadBytesData(stream);
					break;
				case "Ctrl":
					Form1.Ctrl171.ReadBytesData(stream);
					break;
				case "Navd":
					Form1.Nav171.ReadBytesData(stream);
					break;


			}
		}


		public string ReadTag(Stream stream)
		{
			var rawBytes = new byte[4];
			stream.Read(rawBytes, 0, 4);
			string rb = Encoding.ASCII.GetString(rawBytes, 0, 4);
			//Debug.WriteLine(rb);
			return rb;
		}
		public Int32 ReadLength(Stream stream)
		{
			var rawBytes = new byte[4];
			stream.Read(rawBytes, 0, 4);
			Int32 num = BitConverter.ToInt32(rawBytes, 0);
			//Debug.WriteLine(num);
			return num;
		}
		public void ReadValue(Stream stream, Int32 len)
		{
			var rawBytes = new byte[len];
			stream.Read(rawBytes, 0, len);
		}
		public List<string> GetProperty<T>() where T : class
		{

			List<string> propList = new List<string>();

			// get all public static properties of MyClass type
			PropertyInfo[] propertyInfos;
			propertyInfos = typeof(T).GetProperties(BindingFlags.Public |
															BindingFlags.Instance);
			// sort properties by name
			Array.Sort(propertyInfos,
					delegate (PropertyInfo propertyInfo1, PropertyInfo propertyInfo2) { return propertyInfo1.Name.CompareTo(propertyInfo2.Name); });

			// write property names
			foreach (PropertyInfo propertyInfo in propertyInfos)
			{
				//Debug.WriteLine(propertyInfo.Name);
				propList.Add(propertyInfo.Name);
			}

			return propList;
		}
		
	}
}
