// thread.cix

struct Thread
{
	ThreadLocalIBCInfo* m_pIBCInfo;
}

struct ThreadLocalInfo
{
	Thread* m_pThread;
	AppDomain* m_pAppDomain;
	void** m_EETlsData;
	Compiler* m_pCompiler;
}

struct ThreadLocalIBCInfo
{
	bool m_fProcessingDelayedList;
	bool m_fCallbackField;
	bool m_fLoggingDisabled;
	int m_iMinCountToProcess;
	void* m_pDelayList;
}

struct AppDomain
{
	int padding;
}

struct Compiler
{
	int padding;
}

// Thread functions

Thread* GetThread()
{
	ThreadLocalInfo* gCurrentThreadInfo =  (ThreadLocalInfo*)GetGlobal("gCurrentThreadInfo");
	return gCurrentThreadInfo->m_pThread;
}

ThreadLocalIBCInfo* GetIBCInfo(Thread* thread)
{
	return thread.m_pIBCInfo;
}

ThreadLocalIBCInfo* ConstructThreadLocalIBCInfoPtr()
{
	ThreadLocalIBCInfo* result = (ThreadLocalIBCInfo*)malloc(sizeof(ThreadLocalIBCInfo));
	result->m_fCallbackFailed = false;
	result->m_fProcessingDelayedList = false;
	result->m_fLoggingDisabled = false;
	result->m_iMinCountToProcess = 8;
	result->m_pDelayList = NULL;
	return result;
}

bool IsDisabled(ThreadLocalIBCInfo* this)
{
	return this.m_fLoggingDisabled || IsInCantAllocRegion();
}

void CallbackHelper(ThreadLocalIBCInfo* this, void* p, funcptr callback)
{
	invoke_funcptr(callback);
}

void SetIBCInfo(ThreadLocalIBCInfo* pInfo)
{
	m_pIBCInfo = pInfo;
}