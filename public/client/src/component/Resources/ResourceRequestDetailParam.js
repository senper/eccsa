import React, { useState } from "react";
import { makeStyles } from "@material-ui/core/styles";
import { Grid } from "@material-ui/core";
import "../../App.css";

import Footer from "../../component/Footer/Footer";
import { useHistory, useParams } from "react-router-dom";

const useStyle = makeStyles((theme) => ({
  main: {
    width: "100%",
    background: "#fff",
    padding: "10px",
  },
}));

export default function ResourceRequestDetailParam() {
  const history = useHistory();
  const [serviceTypeId, setServiceTypeId] = useState(null);
  const [requestId, setRequestId] = useState(null);
  const classes = useStyle();
  let params = useParams();
  React.useEffect(() => {
    if (params.serviceTypeId) {
      setServiceTypeId(params.serviceTypeId.trim());
    }
    if (params.requestId) {
      setRequestId(params.requestId.trim());
    }
  }, [params]);

  function Redirect() {
    history.push({
      pathname: "/resourceRequestDetail",
      state: {
        serviceRequestId: requestId,
        serviceTypeId:serviceTypeId,
      },
    });
  }
  return (
    <div class="mmenu">
      <div className={classes.main}>
        <Grid container spacing={2} className="grid">
          <Grid item xs={3} md={3}></Grid>
          <Grid item xs={6} md={6}>
            {!serviceTypeId && !requestId ? (
              <h3>Invalid Request</h3>
            ) : (
              Redirect()
            )}
          </Grid>
          <Grid item xs={3} md={3} className={classes.counter}></Grid>

          <br />
          <br />
          
        </Grid>
      </div>
    </div>
  );
}
