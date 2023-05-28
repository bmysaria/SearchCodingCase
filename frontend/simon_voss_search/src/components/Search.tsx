import {Box, Container, Grid, InputAdornment, List, ListItem, styled, TextField} from "@mui/material";
import { useState } from "react";
import SearchIcon from "@mui/icons-material/Search";
import { SearchResult } from "../model/SearchResult";
import ApartmentSharpIcon from '@mui/icons-material/ApartmentSharp';
import LockSharpIcon from '@mui/icons-material/LockSharp';
import GroupSharpIcon from '@mui/icons-material/GroupSharp';
import Person2SharpIcon from '@mui/icons-material/Person2Sharp';

const Search =() => {

    const [searchTerm, setSearchTerm] = useState<string>("");
    const [searchResults, setSearchResults] = useState<SearchResult[]>([]);
     async function searchRequest(targetString:string)
    {
        console.log(searchTerm);
        try {
            console.log('TRY');
            const response = await  fetch("https://localhost:7144/api/Search/" + targetString, {
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
                const data= await response.json() as SearchResult[];
                setSearchResults(data);
            }
        }
        catch (error)
        {
            console.log(error);
        }
    }

    // @ts-ignore
    const handleChange = (event) => {
        setSearchTerm(event.target.value);
        searchRequest(event.target.value);
    };

        return (
            <div  >
                <Container maxWidth="md" sx={{mt: 20}}>
                    <TextField
                        id="search"
                        type="search"
                        label="Search"
                        variant="filled"
                        value={searchTerm}
                        onChange={handleChange}
                        sx={{width: 700, background: 'rgba(255,255,255,0.6)'}}
                        InputProps={{
                            sx: {
                                color: '#01579b'},
                            endAdornment: (
                                <InputAdornment position="end">
                                    <SearchIcon/>
                                </InputAdornment>
                            ),
                        }}
                    />
                </Container>
                <Box
                    display="flex"
                    justifyContent="center"
                    alignItems="center"

                >
                    {
                        searchResults != undefined && searchTerm != '' &&
                    <List
                        sx={{
                            width: '100%',
                            maxWidth: 700,
                            color:'#01579b',
                            position: 'relative',
                            overflow: 'auto',
                            maxHeight: 300,
                            background: 'rgba(255,255,255,0.6)',

                            '& ul': { padding: 0 },
                        }}
                        subheader={<li />}
                    >
                {
                    // searchResults != undefined && searchTerm != '' &&
                    searchResults.map((x) => (<ListItem>

                        {x.matchedValue}
                        {x.type == 'Building' && <ApartmentSharpIcon/>}
                        {x.type == 'Lock' && <LockSharpIcon/>}
                        {x.type == 'Group' && <GroupSharpIcon/>}
                        {x.type == 'Medium' && <Person2SharpIcon/>}
                        ({x.matchedProperty.toLowerCase()})
                    </ListItem>))
                }
                </List>
                    }
                </Box>
            </div>
        );
    }
export default Search;