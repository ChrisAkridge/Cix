// corecrt_memory.cix

void* memchr(void* _Pv, int _C, size_t _N)
{
	void* i;
	for (i = _Pv, i < (_Pv + _N); i++)
	{
		int* value = (int*)i;
		if (*value == _C) return i;
	}
	
	return NULL;
}