import React from "react";

// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
// core components
import GridContainer from "components/vender/Grid/GridContainer.js";
import GridItem from "components/vender/Grid/GridItem.js";
import Button from "components/vender/CustomButtons/Button.js";
import Card from "components/vender/Card/Card.js";
import CardBody from "components/vender/Card/CardBody.js";
import CardIcon from "components/vender/Card/CardIcon.js";
import CardHeader from "components/vender/Card/CardHeader.js";

import { cardTitle } from "assets/jss/material-dashboard-pro-react.js";
import Info from "@material-ui/icons/Info";
import { Redirect } from "react-router-dom";
import { Link } from "react-router-dom";
import { ASSOCIATIONS_QUERY } from "variables/associations_gql";
import { useQuery } from "urql";
import CustomDataGrid from "components/common/MaterialUIDataGrid/CustomDataGrid";
import { renderCellExpand } from "components/common/MaterialUIDataGrid/renderCellExpand";
import btnstyles from "assets/jss/material-dashboard-pro-react/views/buttonsStyle.js";
import { AdministrationLevels } from "variables/lookups.js";

const styles = {
  cardIconTitle: {
    ...cardTitle,
    marginTop: "15px",
    marginBottom: "0px",
  },
};

const useBtnStyles = makeStyles(btnstyles);
const useStyles = makeStyles(styles);

export default function Associations() {
  const [associations, setAssociations] = React.useState([]);
  const [result, executeQuery] = useQuery({
    query: ASSOCIATIONS_QUERY,
  });
  React.useEffect(() => {
    if (result.data) {
      const associationsData = result.data.associations;

      setAssociations(associationsData);
      //console.log(associationsData);
    }
  }, [result.data]);
  const columns = [
    {
      headerName: "Name",
      field: "nameEng",
      renderCell: renderCellExpand,
      flex: 0.3,
    },
    {
      headerName: "Level",
      field: "associationLevel",
      flex: 0.08,
      //valueGetter: getLevelText,
    },
    {
      headerName: "Region",
      field: "regionId",
      flex: 0.08,
    },
    {
      headerName: "Actions",
      field: "id",
      flex: 0.08,
      filterable: false,
      sortable: false,
      // eslint-disable-next-line react/display-name
      renderCell: (params) => (
        <div className="actions-right">
          {/* use this button to add a like kind of action */}
          <Button
            justIcon
            round
            color="info"
            size="sm"
            className={btnClasses.marginRight}
          >
            <Link
              style={{ color: "white" }}
              to={{
                pathname: "/membership/association_detail",
                state: { id: params.value },
              }}
            >
              <Info className={btnClasses.icons} />
            </Link>
          </Button>
        </div>
      ),
    },
  ];
  const classes = useStyles();
  const btnClasses = useBtnStyles();
  return (
    <GridContainer>
      <GridItem xs={12}>
        <Card>
          <CardHeader color="primary" icon>
            <CardIcon color="primary">{"Associations"}</CardIcon>
          </CardHeader>
          <CardBody>
            <CustomDataGrid dataRows={associations} columns={columns} />
          </CardBody>
        </Card>
      </GridItem>
    </GridContainer>
  );
}
