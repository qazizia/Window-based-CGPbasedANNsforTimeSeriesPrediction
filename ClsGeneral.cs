using System;
using System.Collections;
using System.IO;
namespace Brain2
{
	/// <summary>
	/// 
	/// </summary>
	public class ClsGeneral
	{
		public ClsGeneral()
		{
			// 
			// TODO: Add constructor logic here
			//
		}
		public ArrayList readtextfile(string FileName)
		{
			ArrayList AlData=new ArrayList();
			string StrParentFolderName,StrChildFolderName,StrFileName;
			//\\DirectoryInfo  myDir=new DirectoryInfo(@"d:");
			int i;
			
			//	StrParentFolderName="Population"+""+PopulationID+"";
			//StrParentFolderName="Population"+""+1+"";
			//StrChildFolderName=""+"Chromosome"+""+ChromosomeID+"";
			//StrChildFolderName="Chromosome"+""+ChromosomeID+"";
			
			StrFileName=FileName+ ".txt";
			FileStream Fs=File.OpenRead(StrFileName);
			StreamReader SR=new StreamReader(Fs);
			AlData.Clear();
			while(SR.Peek()>-1)			
			{
				AlData.Add(Convert.ToDouble( SR.ReadLine()));
				//Console.WriteLine(Convert.ToInt32( SR.ReadLine())); 
			}
			SR.Close();
			Fs.Close();
		
			return((ArrayList) AlData.Clone());
		}
		public void WriteTextFile(ArrayList Altext,string FileName)
		{
			int i;
			string StrFileName;
			StrFileName=FileName+ ".txt";

			FileStream fs1 = new FileStream(StrFileName,FileMode.Create,FileAccess.Write);
			StreamWriter  SW=new StreamWriter(fs1);
			
			for(i=0;i<Altext.Count;i++)
			{
				SW.WriteLine(Altext[i]); 
				//						SW.WriteLine("\t");
				//						SW.WriteLine(Convert.ToString(i));
			}
			//Console.WriteLine((int)AlTmp1[j]);
			//			AlBestFitness.Clear();
				
			SW.Close();
			fs1.Close();
			
		}		
			
			 
	}
}
