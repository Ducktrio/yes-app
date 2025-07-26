import useFetchRooms from "@/hooks/useFetchRooms";
import useFetchRoomTypes from "@/hooks/useFetchRoomTypes";
import {
  Drawer,
  DrawerHeader,
  DrawerItems,
  Table,
  TableBody,
  TableCell,
  TableRow,
} from "flowbite-react";

export default function DrawerListAvailables({ open }: { open: boolean }) {
  const types = useFetchRoomTypes();
  const rooms = useFetchRooms({ status: 0 });
  const handleClose = () => {};

  return (
    <Drawer
      open={open}
      position="right"
      onClose={handleClose}
      backdrop={false}
      className="w-96 p-8"
    >
      <DrawerHeader title="Available Rooms" />
      <DrawerItems>
        <Table>
          <TableBody>
            {types.data?.map((type) => (
              <TableRow key={type.id}>
                <TableCell className="font-semibold">{type.name}</TableCell>
                <TableCell>
                  {rooms.data?.filter((x) => x.roomType_id == type.id).length}
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </DrawerItems>
    </Drawer>
  );
}
