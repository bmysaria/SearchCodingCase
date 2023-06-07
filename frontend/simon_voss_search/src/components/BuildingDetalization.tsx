import * as React from 'react';
import ApartmentSharpIcon from '@mui/icons-material/ApartmentSharp';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import {SearchResult} from "../model/SearchResult";
import {Building} from "../model/Building";
import {useState} from "react";
import {Grid, Paper, Typography} from "@mui/material";
const BuildingDetalization = (id : string) =>{
    const [building, setBuilding] = useState<Building>();
    async function detalizationRequest()
    {
        try {
        console.log('TRY');
        const response = await  fetch("https://localhost:7144/api/Detalization/building/" + id, {
            method: 'get',
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json'
            },
            redirect: 'follow',
            referrerPolicy: 'no-referrer',
        });
        if (response.ok)
        {
            const data= await response.json() as Building;
            setBuilding(data);
        }
    }
    catch (error)
        {
            console.log(error);
        }
    }
    return (
        { building != undefined &&
        <Tooltip title="building-tooltip">

            <Grid container spacing={2}>
                <Grid item xs={6} sm={3}>
                    <Paper>
                        <Typography variant="body1">{building.name}</Typography>
                    </Paper>
                </Grid>
                <Grid item xs={6} sm={3}>
                    <Paper>
                        <Typography variant="body1">{building.id}</Typography>
                    </Paper>
                </Grid>
                <Grid item xs={6} sm={3}>
                    <Paper>
                        <Typography variant="body1">{building.shortCut}</Typography>
                    </Paper>
                </Grid>
                <Grid item xs={6} sm={3}>
                    <Paper>
                        <Typography variant="body1">{building.description}</Typography>
                    </Paper>
                </Grid>
            </Grid

        </Tooltip>
}
    );
}

export default BuildingDetalization;