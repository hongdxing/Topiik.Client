﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topiik.Client.Interface
{
    public interface IListCommand
    {
        /*
         * Push value to list header(Left)
         * Parameters:
         *  - key: the key of list
         *  - values: the values
         * Return:
         *  - lenght of list after push
         */
        long LPush(string key, params string[] values);

        /*
         * Push value to list tail(Right)
         * Parameters:
         *  - key: the key of list
         *  - values: the values
         * Return:
         *  - length of list after push
         */
        long LPushR(string key, params string[] values);

        /*
         * Pop value(s) from header(Left)
         * Parameters:
         *  - key: the key of list
         *  - count: how many values to pop from list
         * Return:
         *  - list of values
         */
        List<string> LPop(string key, int count=1);

        /* Pop value(s) from list tail(Right)
         * Parameters:
         *  - key: the key of list
         *  - count: how many values to pop from list
         * Return:
         *  - list of values
         */
        List<string> LPopR(string key, int count=1);

        /*
         * Get length of list
         * Parameters:
         *  - key: the key of list
         * Return:
         *  - length of list
         */
        long LLen(string key);

        /*
         * Return list of elements in list from start to end(exclusive)
         * no change to the list after command executed
         * Parameters:
         *  - key: the key of list
         *  - start: start position
         *  - end: end position
         * Return:
         *  - list of elements in list from start to end(exclusive)
         */
        //List<string> LRange(string key, int start, int end);

        List<string> LSlice(string key, int start, int end);

        String LSet(string key, string value, int index);

    }
}
