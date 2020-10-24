using System;
using System.Collections;
using System.IO;

namespace Brain2
{
	/// <summary>
	/// Summary description for ClsNetworkProcess.
	/// </summary>
	public class ClsNetworkProcess
	{
		ClsChromosome ClsChr=new ClsChromosome();
		ClsMain ClsMn=new ClsMain();
		public ClsNetworkProcess()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public ArrayList NetworkProcess(int PopID,int ChrID,ArrayList AlInputs,int NumberOfOutputs,double[] AlGenotype,ArrayList AlActiveGenes)
		{
			ArrayList AlOutputs=new ArrayList();

			AlOutputs=ClsChr.RunGenoType(AlInputs,AlGenotype,AlActiveGenes,NumberOfOutputs);
			
			return(AlOutputs);
		}
		public ArrayList ListofActiveNodes(double[] AlGenotype)
		{
			ArrayList AlOutputs=new ArrayList();
			ArrayList AlActiveNodes=new ArrayList();
			int i,NodeLocation, NodeNumber,j=0,k,tmp1,tmp2;

			for(i=0;i<ClsMn.NumberOfOutputs;i++)
				AlOutputs.Add(AlGenotype[AlGenotype.Length-1-i]);
			AlOutputs.Sort();
			do
			{
			
				NodeNumber=Convert.ToInt32(AlOutputs[AlOutputs.Count-1-j]);

				NodeLocation=(NodeNumber-ClsMn.NumberofInputs-ClsMn.NumberOfRInputs)*(ClsMn.NumberOfInputPerNode*2+1);
				if(NodeLocation<=0)
					break;
				for(i=0;i<ClsMn.NumberOfInputPerNode*2;i+=2)
					AlOutputs.Add(AlGenotype[NodeLocation+i]);

				AlOutputs.Sort();
				j++;
				for(k=1;k<AlOutputs.Count;k++)
				{
					tmp1=Convert.ToInt32( AlOutputs[k]);
					tmp2=Convert.ToInt32(AlOutputs[k-1]);
					if(tmp1==tmp2)
					{
						AlOutputs.Remove(AlOutputs[k]);
						k--;
					}
				}
			
			}	while(NodeLocation>0);
		
			AlActiveNodes.Add(Convert.ToInt32(AlOutputs[0])); 
			for(i=1;i<AlOutputs.Count;i++)
			{
				while(Convert.ToInt32(AlOutputs[i])==Convert.ToInt32(AlOutputs[i-1]))
				{
					if(i<(AlOutputs.Count-1))
						i++;
					else
						break;
				}
				AlActiveNodes.Add(Convert.ToInt32(AlOutputs[i]));
			}


			return(AlActiveNodes);
		}
	}
}
