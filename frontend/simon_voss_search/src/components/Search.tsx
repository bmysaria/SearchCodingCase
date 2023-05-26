import { Container, InputAdornment, TextField } from "@mui/material";
import { useState } from "react";
import SearchIcon from "@mui/icons-material/Search";
import { SearchResult } from "../model/SearchResult";

const Search =() => {

    const [searchTerm, setSearchTerm] = useState<string>("");
    const [searchResults, setSearchResults] = useState<SearchResult[]>([]);
     async function searchRequest()
    {
        console.log(searchTerm);
        try {
            console.log('TRY');
            const response = await  fetch("https://localhost:7144/api/Search/wc", {
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
        searchRequest();
    };

        return (
            <div>
                <Container maxWidth="md" sx={{mt: 20}}>
                    <TextField
                        id="search"
                        type="search"
                        label="Search"
                        value={searchTerm}
                        onChange={handleChange}
                        sx={{width: 600}}
                        InputProps={{
                            endAdornment: (
                                <InputAdornment position="end">
                                    <SearchIcon/>
                                </InputAdornment>
                            ),
                        }}
                    />
                </Container>
            </div>
        );
    }
export default Search;