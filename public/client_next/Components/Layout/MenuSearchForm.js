import React from "react";
import { Form } from "react-bootstrap";
import Input from "@mui/material/Input";
import InputAdornment from "@mui/material/InputAdornment";
import SearchIcon from "@mui/icons-material/Search";
import { useRouter } from "next/router";

export default function MenuSearchForm() {
  const history = useRouter();
  const [searchText, setSearchText] = React.useState("");
  React.useEffect(() => {
    if (searchText.length > 3) {
      history.push({
        pathname: "/menuSearchResult",
        query: {
          searchKey: searchText,
        },
      });
    }
  }, [searchText]);
  return (
    <Input
      type="text"
      value={searchText}
      onChange={(e) => {
        setSearchText(e.target.value);
      }}
      placeholder="Search"
      variant="outline-info"
      id="input-with-icon-adornment"
      startAdornment={
        <InputAdornment position="start">
          <SearchIcon style={{ fill: "white" }} />
        </InputAdornment>
      }
    />
  );
}
