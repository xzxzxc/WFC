import React from "react";
import Slider from "react-slick";
import './History.css';
import {ICollapseWithHistoryResult} from "./web-api-client";

export default function History({width, height, valuesHistory}: ICollapseWithHistoryResult) {
    return <Slider
        fade={true}
        speed={10}
        autoplay={true}
        autoplaySpeed={10}
        pauseOnHover={true}
    >
        {valuesHistory!.map((state, stateIndex) => (
            <div key={stateIndex}>
            <pre>
                    {state
                        .map((elem, elemIndex) => ((elem ?? '■') + (
                            elemIndex % width! === width! - 1
                                ? '\n'
                                : '')))
                        .join('')}
            </pre>
            </div>
        ))}
    </Slider>;
}