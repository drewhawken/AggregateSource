﻿using System;

namespace AggregateSource.GEventStore.Framework
{
    public static class Catch
    {
        public static void ExceptionOf(Action action)
        {
            // ReSharper disable EmptyGeneralCatchClause
            try
            {
                action();
            }
            catch {}
            // ReSharper restore EmptyGeneralCatchClause
        }
    }
}