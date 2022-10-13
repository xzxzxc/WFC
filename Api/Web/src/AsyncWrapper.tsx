import React from "react";
import {UseAsyncReturn} from "react-async-hook";
import {Client} from "./web-api-client";
export {useAsync} from "react-async-hook";


export const Api = new Client();

export type AsyncWrapperProps<T> = {
    async: UseAsyncReturn<T>,
    children: (data: T) => React.ReactNode
}

export function AsyncWrapper<T>({async, children}: AsyncWrapperProps<T>) {
    return (
        <div>
            {async.loading && <div>Loading</div>}
            {async.error && <div>
              Error:
                {async.error.name}
                {async.error.message}
                {async.error.stack}
            </div>}
            {async.result && (
                <div>
                    {children(async.result)}
                </div>
            )}
        </div>
    )
}