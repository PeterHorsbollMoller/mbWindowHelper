using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DockingFunctions
{
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class UserControl1 : UserControl
	{
		public UserControl1()
		{
			InitializeComponent();

			this.Background = SystemColors.WindowBrush;
		}

		public TextBlock TextBlock1
		{
			get
			{
				return textBlock1;
			}
		}


		private double m_hScaleFactor = -1.0;
		private double m_vScaleFactor = -1.0;

		
		/// <summary>
		/// Gets a scaling factor that a pixel-based host control can use 
		/// to set the (resolution-independent) WPF-based layer control 
		/// to an appropriate pixel size.  When the system has a default DPI 
		/// of 96, this property should have a value of 1. 
		/// </summary>
		public double HorizontalScaleFactor
		{
			get
			{
				if (m_hScaleFactor < 0.0)
				{
					CalculateDPIScaleFactors();
				}
				return m_hScaleFactor;
			}
		}

		/// <summary>
		/// Gets a scaling factor that a pixel-based host control can use 
		/// to set the (resolution-independent) WPF-based layer control 
		/// to an appropriate pixel size.  When the system has a default DPI 
		/// of 96, this property should have a value of 1. 
		/// </summary>
		public double VerticalScaleFactor
		{
			get
			{
				if (m_vScaleFactor < 0.0)
				{
					CalculateDPIScaleFactors();
				}
				return m_vScaleFactor;
			}
		}

		private void CalculateDPIScaleFactors()
		{
			// This is kludgey code.  TODO review, probably rewrite. 
			// I'm trying to determine screen DPI, which I want to know 
			// because the LC is sized incorrectly when the system DPI 
			// is not the standard 96.  When the LcController gets a resize, 
			// it uses these scaling factors to size the LC.  
			Matrix m = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
			double dx = m.M11;
			double dy = m.M22;
			if (dx > 0 && dy > 0)
			{
				m_hScaleFactor = 1 / dx;
				m_vScaleFactor = 1 / dy;
			}
			else
			{
				m_hScaleFactor = 1;
				m_vScaleFactor = 1;
			}
		}

		private void button1_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("LC selection count is " + textBlock1.Text);
		}

		private void UserControl_MouseEnter(object sender, MouseEventArgs e)
		{

		}
	}
}
