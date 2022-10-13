import React from "react";
import {AsyncWrapper, useAsync, Api} from "./AsyncWrapper";

export default function PossibleValuesList() {
    const possibleValues = useAsync(
        (type: string) => Api.getPossibleValues(type),
        ['string']);

    return <AsyncWrapper async={possibleValues} children={valuesArray => (
        <div>
            {valuesArray.map((values, index) => (
                <div key={index}>
                    {values.name}:
                    {values.values!
                        .map(value => value.value)
                        .join(', ')}
                </div>
            ))}
        </div>
    )}/>;
}