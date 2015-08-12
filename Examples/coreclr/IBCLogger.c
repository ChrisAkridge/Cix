// ibclogger.cix

struct IBCLogger
{
	int dwInstrEnabled;
}

struct type
{
	int padding;
}

// IBCLogger methods
void LogNameAccess(type p)
{
	if (InStrEnabled())
	{
		LogNameAccessStatic((void*)p);
	}
}

void LogNameAccessStatic(void* p)
{
	return NULL;
}

void LogAccessThreadSafeHelperStatic(IBCLogger* g_IBCLogger, void* p, funcptr callback)
{
	LogAccessThreadSafeHelper(g_IBCLogger, p, callback)
}

void LogAccessThreadSafeHelper(IBCLogger* ptr, void* p, funcptr callback)
{
	if (p == NULL)
	{
		return;
	}
	
	Thread* pThread = GetThread();
	
	if (pThread == NULL)
	{
		return;
	}
	
	ThreadLocalIBCInfo* pInfo = GetIBCInfo(pThread);
	
	if (pInfo == NULL)
	{
		pInfo = ConstructThreadLocalIBCInfoPtr();
		pThread->SetIBCInfo(pInfo);
	}
	
	if (!IsLoggingDisabled(pInfo))
	{
		CallbackHelper(p, callback);
	}
}

bool InstrEnabled(IBCLogger* ptr)
{
	return (ptr->dwInstrEnabled != 0);
}