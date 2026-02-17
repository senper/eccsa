import React from "react";
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Grid,
  Snackbar,
} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faThumbsUp } from "@fortawesome/free-solid-svg-icons";
import { AddAlert } from "@mui/icons-material";
import {
  getUserBusinessInterest,
  removeBusinessInterest,
  saveBusinessInterest,
} from "../../api/userBusinessInterest";
import UserBusinessInterestSectors from "./UserBusinessInterestSectors";
import { Button, Form } from "react-bootstrap";
import UserBusinessInterestProducts from "./UserBusinessInterestProducts";

export default function UserBusinessInterest({
  expanded = true,
  getLocal,
  getLocalByVal,
}) {
  const [userSectors, setUserSectors] = React.useState([]);
  const [details, setDetails] = React.useState([]);
  const [description, setDescription] = React.useState("");
  const [saving, setSaving] = React.useState(false);
  const [removing, setRemoving] = React.useState(false);
  React.useEffect(() => {
    loadUserInterest();
  }, []);

  const loadUserInterest = () => {
    getUserBusinessInterest().then((data) => {
      if (data.data) {
        if (data.data.description) {
          setDescription(data.data.description);
        } else {
          setDescription("");
        }
        if (data.data.sectors && data.data.sectors.$values) {
          setUserSectors(data.data.sectors.$values);
        } else {
          setUserSectors([]);
        }
        if (data.data.details && data.data.details.$values) {
          setDetails(data.data.details.$values);
        } else {
          setDetails([]);
        }
      } else {
      }
    });
  };
  //
  const [error, setError] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState("");
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
  const saveInterestDetail = () => {
    setSaving(true);
    const data = { description, sectors: userSectors, details };
    saveBusinessInterest(data)
      .then((res) => {
        setSaving(false);
        if (res.data.saved) {
          loadUserInterest();
          setErrorMsg(getLocal("Saved"));
        } else {
          setErrorMsg(res.data.erorMsg);
        }
        setError(true);
        showNotification("tr");
      })
      .catch((error) => {
        setSaving(false);
        // console.log(error);
        showNotification("tr");
      });
  };
  //
  const removeInterest = () => {
    setRemoving(true);
    removeBusinessInterest().then((res) => {
      setRemoving(false);
      if (res.data.saved) {
        loadUserInterest();
        setErrorMsg(getLocal("Removed"));
      }
    });
  };
  return (
    <>
      <Grid item xs={12}>
        <Accordion defaultExpanded={expanded}>
          <AccordionSummary expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}>
            <div className="icon">
              <FontAwesomeIcon icon={faThumbsUp} size="1x" />
            </div>
            <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
              <p className="links"> {getLocal("Business")} {getLocal("Interest")}</p>
            </div>
          </AccordionSummary>
          <AccordionDetails style={{ display: "block" }}>
            <Button
              variant="outline-danger"
              style={{ float: "right", margin: "0px", zIndex: "0" }}
              onClick={removeInterest}
              size="sm"
              disabled={saving || removing}
            >
              {removing ? `${getLocal("Removing")} ...` : getLocal("Remove")}
            </Button>
            <Button
              variant="outline-primary"
              style={{ float: "left", margin: "0px", zIndex: "0" }}
              onClick={saveInterestDetail}
              disabled={saving || removing}
            >
              {saving ? `${getLocal("Saving")} ...` : getLocal("Save")}
            </Button>
            <br />
            <br />
            <strong>Description</strong>
            <Form.Control
              as="textarea"
              rows="2"
              required={true}
              value={description}
              placeholder={getLocalByVal("Describe your Business Interest","የንግድ ፍላጎቶን ባጭሩ ይግለጹ")}
              onChange={(e) => setDescription(e.target.value)}
            />
            <UserBusinessInterestSectors
              userSectors={userSectors}
              setUserSectors={setUserSectors}
              setError={setError}
              setErrorMsg={setErrorMsg}
              showNotification={showNotification}
              getLocal={getLocal}
              getLocalByVal={getLocalByVal}
            />

            <UserBusinessInterestProducts
              userProducts={details}
              setUserProducts={setDetails}
              setError={setError}
              setErrorMsg={setErrorMsg}
              showNotification={showNotification}
              getLocal={getLocal}
              getLocalByVal={getLocalByVal}
            />
          </AccordionDetails>
        </Accordion>
      </Grid>
      <Snackbar
        place="tr"
        color="danger"
        icon={AddAlert}
        message={errorMsg ? errorMsg : getLocal("Failed to Save")}
        open={error}
        onClose={() => setError(false)}
      />
    </>
  );
}
