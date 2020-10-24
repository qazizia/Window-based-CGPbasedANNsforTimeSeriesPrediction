using System;
using System.Collections;
using System.IO;

namespace Brain2
{
	/// <summary>
	/// 
	/// </summary>
	public class ClsChromosome
	{
		
		
		
		//ClsGeneral ClsGen=new ClsGeneral();
		//ClsDataBase ClsDB=new ClsDataBase();
		ClsMain ClsMn=new ClsMain();
		
		public ClsChromosome()
		{
			// 
			// TODO: Add constructor logic here
			//
		}
		//Produce Initial Chromosome Population
		public ArrayList ProduceInitialChrPopulation(int PopulationID)
		{
			ArrayList AlPopulation=new ArrayList();
			int NumberOfOffSprings=ClsMn.NumberOfChromosomeOffsprings;//Using 1+Limbda Approach
			int i;
			string StrSelectQuery;
			int PopulationSeed;
			string StrParentFolderName;
			string StrInsertQuery;
			string  StrGender;
			Random RandomClass=new Random(1);
			
			StrParentFolderName="Population"+""+PopulationID+"";
			for(i=0;i<=NumberOfOffSprings;i++)
			{	
				PopulationSeed=RandomClass.Next(1000); 
				StrGender="Agent";
				AlPopulation.Add (ProduceChromosome(PopulationID,i,PopulationSeed,StrGender));
			}
			return(AlPopulation);

		}
		public double[] ProduceChromosome(int PopulationID,int ChromosomeID,int PopulationSeed,string StrGender)
		{
			int i,j;
			ArrayList AlChromosome=new ArrayList();
			string StrFileName;
			string StrParentFolderName,StrChildFolderName;
			
			ArrayList AlTmp1=new ArrayList();
			ArrayList AlTmp2=new ArrayList();
			DirectoryInfo  myDir=new DirectoryInfo(@"d:");
			
			

			//Add all Genotypes to Chromosome
			AlChromosome.Add(ProduceGenoType(PopulationSeed,ClsMn.NumberofInputs+ClsMn.NumberOfRInputs,ClsMn.NumberOfOutputs));
			
			if(PopulationID%10==0)
			{
				StrParentFolderName="Population"+""+PopulationID+"";	
				myDir.CreateSubdirectory(StrParentFolderName);
				myDir=new DirectoryInfo(StrParentFolderName);
				StrChildFolderName=""+"Chromosome"+""+ChromosomeID+"";
				myDir.CreateSubdirectory(StrChildFolderName);
			
				//Write the Chromosome to the file
				for(i=0;i<AlChromosome.Count;i++)
				{
					StrFileName="./"+""+StrParentFolderName+""+"/"+""+StrChildFolderName+""+"/"+"GenoType"+""+(i+1)+""+".txt";

					FileStream fs = File.Create(StrFileName);
					StreamWriter  SW=new StreamWriter(fs);
					AlTmp1=(ArrayList)AlChromosome[i];
					for(j=0;j<AlTmp1.Count;j++)
					{
						//AlTmp2=(ArrayList)AlTmp1[j];
						//for(k=0;k<AlTmp2.Count;k++)
						SW.WriteLine((int)AlTmp1[j]); 
						//Console.WriteLine((int)AlTmp1[j]);
					}
					SW.Close();
					fs.Close();
				}
			}
			
			return((double[])AlChromosome[0]);
			
		}
		//Generate GenoType
		public double[] ProduceGenoType(int PopulationSeed,int NumberOfInputs,int NumberofOutputs)
		{
			int i,j;
			ArrayList AlNodeFunctions=new ArrayList(ClsMn.NumberofNodesPerChromosome); //Represent the Numbers List which Shows the FunctionList
			ArrayList AlGenCode=new ArrayList();//Represents the Output Numbers from where we can take the output Values of the Genotype
			ArrayList AlNodes=new ArrayList();//Store each node with inputsNumbers as a single object
			double[] AlGenoType=new double[ClsMn.NumberofNodesPerChromosome*(ClsMn.NumberOfInputPerNode*2+1)+ClsMn.NumberOfOutputs];//Represent the List of All nodes inputs, weights and Functions in a single List
			
			//Random Class Declaration with Seed 5.
			Random  RandomClass=new Random(PopulationSeed);
		
			//Get the Random NodeFunctions List
			AlNodeFunctions=ClsMn.RandomList(0,ClsMn.NumberOfFunctions-1,1,ClsMn.NumberofNodesPerChromosome);   
			
			//Get the Functionality of each node with output
			for(i=0;i<ClsMn.NumberofNodesPerChromosome;i++) 
			{
				AlNodes.Clear(); 
				//Select the Inputs of each Node Randomly
				for(j=0;j<ClsMn.NumberOfInputPerNode*2;j+=2)
				{
					//Get the Number of Input to the node
					//AlNodes.Add(RandomClass.Next(InitialNumberOfInputs+j));//Store the Node InputsNumber
					AlGenoType[i*(ClsMn.NumberOfInputPerNode*2 +1)+j]=RandomClass.Next(NumberOfInputs+i);//Store the Node InputsNumber
					//Console.WriteLine(AlGenoType[j]); 
					AlGenoType[i*(ClsMn.NumberOfInputPerNode*2 +1)+j+1]=(Math.Pow(-1,RandomClass.Next(10)))*RandomClass.NextDouble();
					
				}
				//AlNodes.Add(AlNodeFunctions[i]);//Store the Node Function
				AlGenoType[i*(ClsMn.NumberOfInputPerNode*2+1) +j]=Convert.ToDouble( AlNodeFunctions[i]);//Store the Node Function
				
				//Store the First Node
				//	AlGenoType.Add(AlNodes.Clone());
			}//End For Loop with I
			for(i=0 ;i<ClsMn.NumberOfOutputs;i++)
				AlGenoType[ClsMn.NumberofNodesPerChromosome*(ClsMn.NumberOfInputPerNode*2+1)+i]=(RandomClass.Next(NumberOfInputs+ClsMn.NumberofNodesPerChromosome));
			return(AlGenoType);
		}
		//Load Population from the Database
		public ArrayList LoadPopulation(int PopulationID)
		{
			ArrayList AlChromosome=new ArrayList();
			double[] AlGenoType=new double[ClsMn.NumberofNodesPerChromosome*(ClsMn.NumberOfInputPerNode*2 +1)+ClsMn.NumberOfOutputs];
			ArrayList AlPopulation=new ArrayList();
			string StrFileName;
			string StrParentFolderName,StrChildFolderName;
			DirectoryInfo  myDir=new DirectoryInfo(@"d:");
			int i,ChromosomeID,j;
			
			//	StrParentFolderName="Population"+""+PopulationID+"";
			StrParentFolderName="Population"+""+PopulationID+"";
			for(ChromosomeID=1;ChromosomeID<=(ClsMn.NumberOfChromosomeOffsprings+1);ChromosomeID++)
			
			{
				j=0;
				StrChildFolderName=""+"Chromosome"+""+ChromosomeID+"";
				//StrChildFolderName="Chromosome"+""+ChromosomeID+"";
				for(i=0;i<1;i++)
				{
					StrFileName="./"+""+StrParentFolderName+""+"/"+""+StrChildFolderName+""+"/"+"GenoType"+""+1+""+".txt";
					FileStream Fs=File.OpenRead(StrFileName);
					StreamReader SR=new StreamReader(Fs);
					
					while(SR.Peek()>-1)			
					{
						AlGenoType[j++]=(Convert.ToDouble( SR.ReadLine()));
						//Console.WriteLine(Convert.ToInt32( SR.ReadLine())); 
					}
					SR.Close();
					Fs.Close();
				}
				AlPopulation.Add(AlGenoType.Clone());	
				
				
			}
			return(AlPopulation);
		}
		
		//Load Chromosome from the Database
		public ArrayList LoadChromosome(int PopulationID,int ChromosomeID,int NetworkID)
		{
			ArrayList AlChromosome=new ArrayList();
			ArrayList AlGenoType=new ArrayList();
			string StrFileName;
			string StrParentFolderName,StrChildFolderName;
			DirectoryInfo  myDir=new DirectoryInfo(@"d:");
			int i;
			
			//	StrParentFolderName="Population"+""+PopulationID+"";
			StrParentFolderName="Population"+""+1+"";
			StrChildFolderName=""+"Chromosome"+""+ChromosomeID+"";
			//StrChildFolderName="Chromosome"+""+ChromosomeID+"";
			for(i=0;i<1;i++)
			{
				StrFileName="./"+""+StrParentFolderName+""+"/"+""+StrChildFolderName+""+"/"+"GenoType"+""+PopulationID+""+".txt";
				FileStream Fs=File.OpenRead(StrFileName);
				StreamReader SR=new StreamReader(Fs);
				AlGenoType.Clear();
				while(SR.Peek()>-1)			
				{
					AlGenoType.Add(Convert.ToInt32( SR.ReadLine()));
					//Console.WriteLine(Convert.ToInt32( SR.ReadLine())); 
				}
				SR.Close();
				Fs.Close();
				AlChromosome.Add(AlGenoType.Clone());	
			}
			return((ArrayList) AlGenoType.Clone());
		}

		//Run the Life Cycle GenoType
		public ArrayList RunGenoType(ArrayList AlInput,double[] AlGenoType,ArrayList AlActiveGenes,int NumberOfOutputs)
		{
			int i,j,DesireNumberOfOutputs;
			ArrayList AlResults=new ArrayList();
			ArrayList AlGenCode=new ArrayList();//Represents the Output Numbers from where we can take the output Values of the Genotype
			double[] AlAvailableInputs=new double[ClsMn.NumberofNodesPerChromosome+ClsMn.NumberofInputs+ClsMn.NumberOfRInputs];//Represent the values of the Available Inputs at each node
			ArrayList AlNodeOutputs=new ArrayList();//Represents the Individual NodeOutput Values of the Node
			ArrayList AlNodeInputs=new ArrayList();//Represents the inputvalues of Inputs of Nodes
			int temp;
			int NodeFunction,NodeNumber;
			double a,b,c,d,e,NodeOutput=0,boltmp;
			int In1,In2,In3,In4,In5;	
			double W1,W2,W3,W4,W5;	
			int index;
			//Random Class Declaration with Seed 5.
			//	Random  RandomClass=new Random(5);
			//Get the Random List of Points in Genotype from where i can take my output
			//AlGenCode=ClsGen.RandomList(0,AlInput.Count+ClsMn.NumberOfNodesPerChromosome-1,2,DesiredNumberOfOutputs);  
			DesireNumberOfOutputs=NumberOfOutputs;
			AlGenCode.Clear();
			//			AlAvailableInputs.Clear();
			
			//Assign all the Input values to the Available input
			//			for(j=0;j<;j++)
			//			{
			//				for(i=0;i<AlInput.Count;i++)
			//				{
			//					temp=(int)AlInput[i];
			//					temp=temp>>j;
			//					if((temp & 0x0001)==1)
			//						boltmp=true;
			//					else
			//						boltmp=false;
			//					AlAvailableInputs[j*AlInput.Count+i]=boltmp;
			//					
			//				}
			//			}
			for(i=0;i<AlInput.Count;i++)
				AlAvailableInputs[i]=(double)AlInput[i];
			//Get the Functionality of each node with output
			for(i=0;i<AlActiveGenes.Count;i++) 
			{
				NodeNumber=(int)AlActiveGenes[i];
				if(NodeNumber<(ClsMn.NumberofInputs+ClsMn.NumberOfRInputs))
					goto end;
				else
				{
					index=(NodeNumber-ClsMn.NumberofInputs-ClsMn.NumberOfRInputs)*(ClsMn.NumberOfInputPerNode*2+1);
					In1=(int)AlGenoType[index];
					W1=(double)AlGenoType[index+1];
					In2=(int)AlGenoType[index+2];
					W2=(double)AlGenoType[index+3];
					In3=(int)AlGenoType[index+4];
					W3=(double)AlGenoType[index+5];
					In4=(int)AlGenoType[index+6];
					W4=(double)AlGenoType[index+7];
					In5=(int)AlGenoType[index+8];
					W5=(double)AlGenoType[index+9];

					NodeFunction=(int)AlGenoType[index+10];
			
					
					a=(double)AlAvailableInputs[In1];
					b=(double)AlAvailableInputs[In2];
					c=(double)AlAvailableInputs[In3];
					d=(double)AlAvailableInputs[In4];
					e=(double)AlAvailableInputs[In5];
						
					NodeOutput=1/(1+Math.Exp(-(a*W1+b*W2+c*W3+d*W4+e*W5)));
					//						switch(NodeFunction)
					//						{
					//							case 0: NodeOutput=1/(1+Math.Exp(-(a+b+c)));
					//							case 1: NodeOutput=Math.Tanh(a+b+c);
					//							case 0: NodeOutput=Convert.ToInt32(System.Math.Sqrt(a+b*c));   
					//								break;
					//							case 1: NodeOutput=Convert.ToInt32(System.Math.Sqrt(a*b+c));
					//								break;
					//							case 2: NodeOutput=Convert.ToInt32(System.Math.Sqrt(Math.Abs(a*b-c)));   
					//								break;
					//							case 3: NodeOutput=Convert.ToInt32(System.Math.Sqrt(a+b*c));   
					//								break;

					//Multiplexer standard equations


					//							case 0: NodeOutput=((a&(~c)|b&c));   
					//								break;
					//							case 1: NodeOutput=((((~a)&(~c)|b&c)));
					//								break;
					//							case 2: NodeOutput=((a&(~c)|(~b)&c));   
					//								break;
					//							case 3: NodeOutput=((((~a)&(~c))|(~b)&c) );   
					//								break;
					//							case 0: NodeOutput=((a&b&c));   
					//								break;
					//							case 1: NodeOutput=((a|b|c));
					//								break;
					//							case 2: NodeOutput=(~(a&b&c));   
					//								break;
					//							case 3: NodeOutput=(~(a|b|c)) ;   
					//								break;
					//						}//End Switch
				
					AlAvailableInputs[NodeNumber]=NodeOutput;
				}
			end:NodeOutput=NodeOutput;
			}//End For Loop with I

			//			for(i=0;i<DesireNumberOfOutputs;i++)
			//			{
			//				temp=(int)AlGenoType[ClsMn.NumberofNodesPerChromosome*4+i];
			//				for(j=0;j<ClsMn.TypesOfInputs;j++)
			//				{
			//					boltmp=(int)AlAvailableInputs[temp*ClsMn.TypesOfInputs+j];
			//					if(boltmp==true)
			//						AlResults.Add(1);
			//					else
			//						AlResults.Add(0);
			//				}
			//			}//End of For Loop with I
			for(i=0;i<DesireNumberOfOutputs;i++)
			{
				temp=(int)AlGenoType[ClsMn.NumberofNodesPerChromosome*(ClsMn.NumberOfInputPerNode*2+1) +i];
				AlResults.Add ((double)AlAvailableInputs[temp]);
					
			}
			return(AlResults);
	
		}
	}
}
