<?php

namespace App\Http\Controllers;

use App\Models\HSX;
use Illuminate\Http\Request;
use Illuminate\Support\Str;

class HSXController extends Controller
{
    /**
     * Display a listing of the resource.
     */
    public function getDanhSach()
    {
        $hsx = HSX::all();
        return view('hsx.danhsach', compact('hsx'));
    }
    public function getThem()
    {
        return view('hsx.them');
    }
    public function postThem(Request $request)
    {
        $orm = new HSX();
        $orm->tenhsx = $request->tenhsx;
        $orm->tenhsx_slug = Str::slug($request->tenhsx, '-');
        $orm->sodienthoai = $request->sodienthoai;
        $orm->diachi= $request->diachi;
        $orm->save();
        return redirect()->route('hsx');
    }
    public function getSua($id)
    {
        $hsx = HSX::find($id);
        return view('hsx.sua', compact('hsx'));
    }
    public function postSua(Request $request, $id)
    {
        $orm = HSX::find($id);
        $orm->tenhsx = $request->tenhsx;
        $orm->tenhsx_slug = Str::slug($request->tenhsx, '-');
        $orm->sodienthoai = $request->sodienthoai;
        $orm->loai = $request->loai;
        $orm->save();
        return redirect()->route('hsx');
    }
    public function getXoa($id)
    {
        $orm = HSX::find($id);
        $orm->delete();
        return redirect()->route('hsx');
    }
}
