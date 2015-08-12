// clrhost.cix

bool IsInCantAllocRegion()
{
	size_t count = 0;
	if (ClrFlsCheckValue(15, (void*)&count))
	{
		return count > 0;
	}
	return false;
}

bool ClrFlsCheckValue(int slot, void** pValue)
{
	return false;
}