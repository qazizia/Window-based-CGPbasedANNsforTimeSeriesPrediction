using System;
using System.Collections;
using System.IO; 

namespace Brain2
{
	/// <summary>
	/// Summary description for ClsMutation.
	/// </summary>
	public class ClsMutation
	{
		
		ClsMain ClsMn=new ClsMain();
		//ClsGeneral ClsGen=new ClsGeneral();
		
		public ClsMutation()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		//Check for the Best Fitted Chromosome and Promote it to Next Generation
		public  ArrayList SelectTheFitChromosome(int MutationSeed,int GenerationID,ArrayList AlFittness,ArrayList AlPopulation)
		{
			int ChromosomeID;
			ArrayList AlData=new ArrayList();
			double[] AGenotype=new double[ClsMn.NumberofNodesPerChromosome*(ClsMn.NumberOfInputPerNode *2 +1)+ClsMn.NumberOfOutputs];
			//ArrayList AlPopulation=new ArrayList();
			double Fitness=0,BestFitness=0;
			int BestChromosomeID=1;
			int i,TargetOutput;
			int AverageValue,AdaptiveMutationRate;
			string line;
			
			
			//Select the Best one
			BestFitness=Convert.ToDouble( AlFittness[0]);
			
			//Select the Best one
			for(i=1;i<AlFittness.Count;i++)
			{
				if(BestFitness>Convert.ToDouble(AlFittness[i]))
				{
					BestFitness=Convert.ToDouble(AlFittness[i]);
					BestChromosomeID=i+1;
				}

			}
			AGenotype=(double[])AlPopulation[BestChromosomeID-1];
			
			//for this case to avoid noise
			//			if(BestFitness==0)
			//				BestChromosomeID=5;
			
			//			//Put the Best Fitness into database
			// 
			//			AdaptiveMutationRate=Convert.ToInt32((ClsMn.MutationRate-(BestFitness*ClsMn.MutationRate)/(ClsMn.DesiredFitness))*2); 
			//			if(AdaptiveMutationRate<5)
			AdaptiveMutationRate=10;
			AlPopulation= NewPopulaton(BestChromosomeID,MutationSeed,1,GenerationID,AGenotype,AdaptiveMutationRate);
			if(GenerationID%1000==0)
				SavePopulation(AlPopulation,GenerationID);
			//			FileStream fs = new FileStream(StrFileName,FileMode.Open,FileAccess.ReadWrite);
			//			StreamReader RD=new StreamReader(fs);
			//			while((line = RD.ReadLine()) != null)
			//				AlData.Add(line);
			AlPopulation.Add(BestChromosomeID); 
			return(AlPopulation);
		}
		//Produce New Population
		public ArrayList NewPopulaton(int MaxChromosomeID,int MutationSeed,int DatabaseID,int PopulationID,double[] AlChromosome,int AdaptiveMutationRate)
		{
			int i,j,k;
			int ChromosomeID;
			string StrParentFolderName,StrChildFolderName,StrFileName;
			ArrayList AlPopulation=new ArrayList();
			double[] AlChrList=new double[ClsMn.NumberofNodesPerChromosome*(ClsMn.NumberOfInputPerNode*2 +1)+ClsMn.NumberOfOutputs];
			object[] objChromosome=new object[7];
			ArrayList AlTmp1=new ArrayList();
			ArrayList AlTmp2=new ArrayList();
			DirectoryInfo  myDir=new DirectoryInfo(@"d:");
			string StrGender="";
			
			ClsChromosome ClsChm=new ClsChromosome();
			
			//			if(DatabaseID==1)
			//				PopulationID=PopulationID+1;
			//			//StrParentFolderName="Population"+""+PopulationID+"";
			//			StrParentFolderName="Population"+""+1+"";
			//			if(DatabaseID==1)
			//				myDir.CreateSubdirectory(StrParentFolderName);
			//			myDir=new DirectoryInfo(StrParentFolderName);
			//			//Store the New Population and Start the Process Again.
			for(i=0;i<=ClsMn.NumberOfChromosomeOffsprings;i++)
			{
				//				AlChromosome.Clear(); 
				//				//Load Chromosome from previous Agent folder
				//				AlChromosome=ClsChm.LoadChromosome(PopulationID-1,MaxChromosomeID,DatabaseID);
				for(j=0;j<AlChromosome.Length;j++) 
					AlChrList[j]=AlChromosome[j]; 
				//Produce Four More Coppies of This Through Mutation and Put them all in Database
				if(i<ClsMn.NumberOfChromosomeOffsprings)
					AlChrList= MutateChromosome(AlChrList,MutationSeed+10*i,AdaptiveMutationRate);
				
				ChromosomeID=i+1;
				AlPopulation.Add((double[])AlChrList.Clone() ); 			
				//				StrChildFolderName=""+"Chromosome"+""+ChromosomeID+"";
				//				myDir.CreateSubdirectory(StrChildFolderName);
				//				//Write the Chromosome to the file
				//				for(j=0;j<1;j++)
				//				{
				//					StrFileName="./"+""+StrParentFolderName+""+"/"+""+StrChildFolderName+""+"/"+"GenoType"+""+(""+PopulationID+"")+""+".txt";
				//
				//					FileStream fs = File.Create(StrFileName);
				//					StreamWriter  SW=new StreamWriter(fs);
				//					AlTmp1=AlChromosome;
				//					for(k=0;k<AlTmp1.Count;k++)
				//					{
				//						//AlTmp2=(ArrayList)AlTmp1[j];
				//						//for(k=0;k<AlTmp2.Count;k++)
				//						SW.WriteLine((int)AlTmp1[k]); 
				//						//Console.WriteLine((int)AlTmp1[j]);
				//					}
				//					SW.Close();
				//					fs.Close();
			
				//				}
			}
			return(AlPopulation);
		}
		//Function for Mutation of Population
		public double[] MutateChromosome(double[] AlChromosome,int MutationSeed,int AdaptiveMutationRate)
		{
			double[] AlGenotype=new double[ClsMn.NumberofNodesPerChromosome*(ClsMn.NumberOfInputPerNode*2 +1)+ClsMn.NumberOfOutputs];
			int NumberofGensToBeMutated=0,OutputGenesToBeMutated;	
			int LocationOfMutation;
			int i;
			int NumberOfGenes;
			int WhichGene;
			int NumberOfinputs;
			int NumberOfGenesPerGenotype=4;
			int WhichNode;
			double MutatedNumber;
			int j,Connection;

			//Random Class Instantiation with Seed=8
			Random RandomClass=new Random(MutationSeed); 
			NumberOfGenes =ClsMn.NumberofNodesPerChromosome*(ClsMn.NumberOfInputPerNode*2+1)+ClsMn.NumberOfOutputs ;//Number of Genotype*(Number of input per node+1)*Number of Nodes 
			NumberofGensToBeMutated= ClsMn.MutationRate*NumberOfGenes/100;	//Calculate the Number of Genes to be mutated
			
			for(j=0;j<AlChromosome.Length;j++)
				AlGenotype[j]=AlChromosome[j]; 
				
			for(i=0;i<NumberofGensToBeMutated;i++)
			{
				LocationOfMutation=RandomClass.Next(1,NumberOfGenes)-1;
				//WhichGenotype=LocationOfMutation/400; 
				NumberOfinputs=ClsMn.NumberofInputs+ClsMn.NumberOfRInputs; 
						
				WhichGene=LocationOfMutation/(ClsMn.NumberofNodesPerChromosome*(ClsMn.NumberOfInputPerNode*2+1));
				if(WhichGene>0)
					MutatedNumber=RandomClass.Next(ClsMn.NumberofNodesPerChromosome+NumberOfinputs-1);//Gene to be mutated is an output gene
				else
				{
					WhichNode=(LocationOfMutation/(ClsMn.NumberOfInputPerNode*2+1));//To locate the node
					if((LocationOfMutation)%(ClsMn.NumberOfInputPerNode*2+1) ==ClsMn.NumberOfInputPerNode*2)
					{
						MutatedNumber=RandomClass.Next(ClsMn.NumberOfFunctions);
					}
					else 
					{
						Connection =(LocationOfMutation)%(ClsMn.NumberOfInputPerNode*2+1);
						if(Connection%2==0)
							MutatedNumber= RandomClass.Next(WhichNode+NumberOfinputs-1);
						else
							MutatedNumber= (Math.Pow(-1,RandomClass.Next(10)))*RandomClass.NextDouble();
								
					}
				}
				AlGenotype[(LocationOfMutation)]=MutatedNumber; 
					
			}
			//			//Output Gene Mutation
			//		OutputGenesToBeMutated= ClsMn.MutationRate*2*ClsMn.NumberOfOutputs/100;	//Calculate the Number of Genes to be mutated
			//			
			//			for(i=0;i<OutputGenesToBeMutated;i++)
			//			{
			//				LocationOfMutation=RandomClass.Next(NumberOfGenes,NumberOfGenes+ClsMn.NumberOfOutputs);
			//				//WhichGenotype=LocationOfMutation/400; 
			//				NumberOfinputs=ClsMn.NumberofInputs; 
			//						
			//				WhichGene=LocationOfMutation/(ClsMn.NumberofNodesPerChromosome*(ClsMn.NumberOfInputPerNode*2+1));
			//				if(WhichGene>0)
			//					MutatedNumber=RandomClass.Next(ClsMn.NumberofNodesPerChromosome+NumberOfinputs-1);//Gene to be mutated is an output gene
			//				else
			//				{
			//					WhichNode=(LocationOfMutation/(ClsMn.NumberOfInputPerNode*2+1));//To locate the node
			//					if((LocationOfMutation)%(ClsMn.NumberOfInputPerNode*2+1) ==ClsMn.NumberOfInputPerNode*2)
			//					{
			//						MutatedNumber=RandomClass.Next(ClsMn.NumberOfFunctions);
			//					}
			//					else 
			//					{
			//						Connection =(LocationOfMutation)%(ClsMn.NumberOfInputPerNode*2+1);
			//						if(Connection%2==0)
			//							MutatedNumber= RandomClass.Next(WhichNode+NumberOfinputs-1);
			//						else
			//							MutatedNumber= (Math.Pow(-1,RandomClass.Next(10)))*RandomClass.NextDouble();
			//								
			//					}
			//				}
			//				AlGenotype[(LocationOfMutation)]=MutatedNumber; 
			//					
			//			}
			return(AlGenotype);
		}
		public void SavePopulation(ArrayList AlPopulation,int PopulationID)
		{
			int i,j,ChromosomeID;
			double[] AGenotype=new double[ClsMn.NumberofNodesPerChromosome*(ClsMn.NumberOfInputPerNode*2+1)+ClsMn.NumberOfOutputs];
			
			string StrFileName;
			string StrParentFolderName,StrChildFolderName;
			
			DirectoryInfo  myDir=new DirectoryInfo(@"d:");
			
			StrParentFolderName="Population"+""+PopulationID+"";	
			myDir.CreateSubdirectory(StrParentFolderName);
			myDir=new DirectoryInfo(StrParentFolderName);
			
			for(ChromosomeID=1;ChromosomeID<=(ClsMn.NumberOfChromosomeOffsprings+1);ChromosomeID++)
			{
				StrChildFolderName=""+"Chromosome"+""+ChromosomeID+"";
				myDir.CreateSubdirectory(StrChildFolderName);
				AGenotype=(double[])AlPopulation[ChromosomeID-1];
				//Write the Chromosome to the file
				
				StrFileName="./"+""+StrParentFolderName+""+"/"+""+StrChildFolderName+""+"/"+"GenoType"+""+(1)+""+".txt";

				FileStream fs = File.Create(StrFileName);
				StreamWriter  SW=new StreamWriter(fs);
				for(j=0;j<AGenotype.Length;j++)
				{
					//AlTmp2=(ArrayList)AlTmp1[j];
					//for(k=0;k<AlTmp2.Count;k++)
					SW.WriteLine(AGenotype[j]); 
					//Console.WriteLine((int)AlTmp1[j]);
				}
				SW.Close();
				fs.Close();
			}
		}
	}
}

