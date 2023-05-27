import React from 'react';
import logo from './logo.svg';
import './App.css';
import Search from "./components/Search";

function App() {

  return (
    <div className="App"
         style={{
             backgroundImage:  `url("https://www.simons-voss.com/_Resources/Persistent/c/5/5/7/c557cd74b99a75d1d19bc00789ae011a6b730d59/system-360-header-visual-muster.jpg")` ,
             backgroundSize: 'cover',
             width:'100%',
             height: '100%',
             position:'absolute'
         }}>
        <Search></Search>
    </div>
  );
}

export default App;
