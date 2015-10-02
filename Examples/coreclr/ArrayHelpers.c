// arrayhelpers.cix

#include <ArrayBase.cix>
#include <corecrt_memory.cix>
#include <MethodTable.cix>
#include <IBCLogger.cix>
#include <Thread.cix>
#include <CLRHost.cix>
#include <Core.cix>

int IndexOfUInt8(byte* array, uint index, uint count, byte value)
{
	char* pvalue = (char*)memchr(array + index, value, count);
	
	if (pvalue == NULL)
	{
		return -1;
	}
	else
	{
		return (int32)(pvalue - array);
	}
}

bool TrySZIndexOf(ArrayBase* array, 