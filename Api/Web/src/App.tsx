import React from 'react';
import './App.css';
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import {CollapseCommand, Element} from './web-api-client';
import PossibleValuesList from "./PossibleValuesList";
import History from "./History";
import {Api, AsyncWrapper, useAsync} from "./AsyncWrapper";
import {debug} from "util";

const shift = (elements: Element[], dx: number, dy: number) => {
    return elements!.map(elem => new Element({
        x: elem.x! + dx,
        y: elem.y! + dy,
        value: elem.value
    }));
};

let initCommand = new CollapseCommand({
    name: 'Unicode',
    width: 20,
    height: 10,
    elements: shift([
        new Element({
            x: 1,
            y: 0,
            value: '┠'
        }),
        new Element({
            x: 0,
            y: 1,
            value: '┸'
        }),
        new Element({
            x: 1,
            y: 1,
            value: '┨'
        }),
        new Element({
            x: 2,
            y: 1,
            value: '┰'
        }),
        new Element({
            x: 1,
            y: 2,
            value: '┨'
        }),
    ], 5, 3)
});

let square = [
    new Element({
        x: 1,
        y: 0,
        value: '┸'
    }),
    new Element({
        x: 0,
        y: 1,
        value: '┨'
    }),
    new Element({
        x: 2,
        y: 1,
        value: '┠'
    }), new Element({
        x: 1,
        y: 2,
        value: '┰'
    }),
];
initCommand = new CollapseCommand({
    name: 'Unicode2',
    width: 20,
    height: 10,
    elements: [square, shift(square, 3, 2), shift(square, 0, 4)].flat()
});

function App() {
    const history = useAsync(
        (command: CollapseCommand) => Api.postApiCollapseHistory(true, command),
        [initCommand]);

    return (
        <div className="App">
            <PossibleValuesList/>
            <AsyncWrapper
                async={history}
                children={History}
            />
        </div>
    );
}

export default App;
