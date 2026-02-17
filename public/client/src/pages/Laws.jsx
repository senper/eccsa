import React from "react";

import { makeStyles } from "@material-ui/core/styles";
import { Grid } from "@material-ui/core";
import { Link, useLocation } from "react-router-dom";
import Breadcrumbs from "@material-ui/core/Breadcrumbs";
import "bootstrap/dist/css/bootstrap.min.css";
import "../App.css";
import "./Pages.css";
import "../component/slider/HomeSlider";
import "bootstrap/dist/css/bootstrap.css";
import { Card, Form, Button, InputGroup } from "react-bootstrap";
import {
  getLawsList,
  lawCategories,
  lawTypes,
  lawDownloadUrl,
  downloadLaw,
} from "../services/laws";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSearch } from "@fortawesome/free-solid-svg-icons";
import Footer from "../component/Footer/Footer";
import { Snackbar } from "@mui/material";
import { AddAlert } from "@material-ui/icons";
// import  from '@mui/material/TextField';

const useStyle = makeStyles((theme) => ({
  grid: {
    width: "100%",
    margin: "0px",
    padding: "3px",
  },
  main: {
    width: "100%",
    background: "#fff",
    padding: "10px",
  },
  breadcrum: {
    backgroundColor: "#fff",
    padding: "10px",
    marginLeft: "100px",
  },
}));
export default function Laws() {
  const classes = useStyle();
  const location = useLocation();
  const [error, setError] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState(false);
  const showNotification = (place) => {
    switch (place) {
      case "tr":
        if (!error) {
          setError(true);
          setTimeout(function () {
            setError(false);
            setErrorMsg("");
          }, 6000);
        }
        break;
      default:
        break;
    }
  };
  //
  let lawCategoryValue = 0;
  let lawTypeValue = 0;
  if (location.state) {
    const { lawCategory, lawType } = location.state;
    lawCategoryValue = lawCategory;
    lawTypeValue = lawType;
  }
  const [loading, setLoading] = React.useState(false);
  const [selectedLawType, setSelectedLawType] = React.useState(1);
  const [laws, setLaws] = React.useState([]);
  React.useEffect(() => {
    if (lawTypeValue) {
      setSelectedLawType(lawTypeValue);
      loadLaws(lawTypeValue);
    }
  }, [lawTypeValue]);
  React.useEffect(() => {
    if (lawCategoryValue) {
      loadLaws(selectedLawType);
    }
  }, [lawCategoryValue]);
  function searchSubmit(event) {
    event.preventDefault();
    loadLaws(selectedLawType);
  }
  function loadLaws(type) {
    setLaws([]);
    setLoading(true);
    getLawsList(lawCategoryValue, type)
      .then((data) => {
        setLaws(data.data.$values);
        setLoading(false);
      })
      .catch((error) => {
        setLoading(false);
      });
  }
  function downloadDoc(law) {
    downloadLaw(law.id)
      .then((data) => {
        const url = data.request.responseURL; // window.URL.createObjectURL(new Blob([data.data]));
        const link = document.createElement("a");
        link.href = url;
        /* link.setAttribute(
          "download",
          `certificate_${date.getFullYear()}${date.getMonth()}${date.getDay()}${date.getHours()}${date.getMinutes()}${date.getSeconds()}.pdf`
        ); */ //or any other extension
        link.setAttribute("target", `_blank`);
        document.body.appendChild(link);
        link.click();
        link.remove();
      })
      .catch((error) => {
        console.log(error.response);
        if (error.response) {
          setErrorMsg("Failed To Download");
          showNotification("tr");
        }
      });
  }
  return (
    <div class="mmenu root">
      <div className={classes.main}>
        <Grid container spacing={2} className="grid">
          <Grid item xs={3} md={3}>
            <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrum}>
              <Link color="inherit" to="/">
                Home
              </Link>
              <Link color="black" to="#" aria-current="page">
                {lawCategoryValue
                  ? lawCategories.find((s) => s.value == lawCategoryValue).text
                  : null}{" "}
                Laws
              </Link>
            </Breadcrumbs>
          </Grid>
          <Grid item xs={9} md={9}>
            <form onSubmit={searchSubmit}>
              <InputGroup style={{ marginTop: "10px" }}>
                <Form.Select
                  md={1}
                  style={{ maxWidth: "370px" }}
                  value={selectedLawType}
                  onChange={(e) => {
                    setSelectedLawType(e.target.value);
                    loadLaws(e.target.value);
                  }}
                >
                  {lawTypes.map((value) => (
                    <option value={value.value}>{value.text}</option>
                  ))}
                </Form.Select>
                <Button
                  type="submit"
                  style={{ margin: "0" }}
                  variant="outline-secondary"
                  id="button-addon2"
                >
                  <FontAwesomeIcon icon={faSearch} />
                </Button>
              </InputGroup>
            </form>
          </Grid>
          <Grid item xs={12} md={12}>
            <div class="container">
              <div className="row">
                {loading ? <h3>Loading...</h3> : null}
                {laws && laws.length > 0 ? (
                  <ul>
                    {laws.map((row, index) => (
                      <li key={row.id} style={{ marginBottom: "7px" }}>
                        <Link
                          onClick={() => downloadDoc(row)}
                          title={row.brief}
                        >
                          {row.title}
                        </Link>
                      </li>
                    ))}
                  </ul>
                ) : null}
                {!loading && (!laws || laws.length <= 0) ? (
                  <h3>NOTHING FOUND</h3>
                ) : null}
              </div>
            </div>
          </Grid>
        </Grid>

        <Snackbar
          place="tr"
          color="danger"
          icon={AddAlert}
          message={errorMsg ? errorMsg : "Failed to Download"}
          open={error}
          closeNotification={() => setError(false)}
          close
        />
      </div>
    </div>
  );
}
