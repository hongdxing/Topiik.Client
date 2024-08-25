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
         *  - value: the value
         * Return:
         *  - lenght of list after push
         */
        long LPush(string key, string value);

        /*
         * Push value to list tail(Right)
         * Parameters:
         *  - key: the key of list
         *  - value: the value
         * Return:
         *  - length of list after push
         */
        long LPushR(string key, string value);

        /*
         * Pop value(s) from header(Left)
         * Parameters:
         *  - key: the key of list
         *  - count: how many values to pop from list
         * Return:
         *  - list of values
         */
        List<string> LPop(string key, int count);

        /* Pop value(s) from list tail(Right)
         * Parameters:
         *  - key: the key of list
         *  - count: how many values to pop from list
         * Return:
         *  - list of values
         */
        List<string> LPopR(string key, int count);

        /*
         * Get length of list
         * Parameters:
         *  - key: the key of list
         * Return:
         *  - length of list
         */
        long Llen(string key);
    }
}