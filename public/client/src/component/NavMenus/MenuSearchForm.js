import React from "react";
import { Form } from "react-bootstrap";
import Input from "@mui/material/Input";
import InputAdornment from "@mui/material/InputAdornment";
import SearchIcon from "@material-ui/icons/Search";
import { useHistory } from "react-router-dom";

export default function MenuSearchForm() {
  const history = useHistory();
  const [searchText, setSearchText] = React.useState("");
  React.useEffect(() => {
    if (searchText.length > 3) {
      history.push({
        pathname: "/menuSearchResult",
        state: {
            searchKey: searchText,
        },
      });
    }
  }, [searchText]);
  return (
    <Form >
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
    </Form>
  );
}
